using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using dotNetSony9Pin.Pattern;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.Return;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.SenseRequest;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.SenseReturn;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.StatusData;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.SystemControl;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.TimeSenseRequest;

namespace dotNetSony9Pin;

public class Sony9PinMaster : Sony9PinBase
{
    public string model { get; internal set; } = "";

    public string manufacturer { get; private set; } = "Generic";

    public string manufacturerShort { get; private set; } = "Generic";

    private StatusData _statusData = new();

    private TimeCode _timeCode = new();

    public StatusData StatusData
    {
        get => _statusData;

        set
        {
            if (!_statusData.Equals(value))
            {
                RaiseStatusDataChangingHandler(_statusData);
                lock (this)
                {
                    _statusData = value;
                }
                RaiseStatusDataChangedHandler(_statusData);
            }
            RaiseStatusDataReceivedHandler(_statusData);
        }
    }

    public TimeCode TimeCode {
        get => _timeCode;

        set
        {
            if (!_timeCode.Equals(value))
            {
                RaiseTimeDataChangingHandler(SenseReturn.LtcTimeData, _timeCode);
                lock (this)
                {
                    _timeCode = value;
                }
                RaiseTimeDataChangedHandler(SenseReturn.LtcTimeData, _timeCode);
            }
            RaiseTimeDataReceivedHandler( SenseReturn.LtcTimeData, _timeCode);
        }
    }

    #region Events and EventHandlers

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<DeviceTypeEventArgs>? OnDeviceType;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<bool>? OnConnectedChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? OnNak;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? OnStatusDataReceived;

    /// <summary>
    ///     The status data.
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? OnStatusDataChanged;

    /// <summary>
    ///     The status data.
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? OnStatusDataChanging;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? OnTimeDataReceived;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? OnTimeDataChanged;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? OnTimeDataChanging;

    /// <summary>
    ///     The raise device type handler.
    /// </summary>
    /// <param name="deviceName">
    ///     The sender.
    /// </param>
    protected virtual void RaiseDeviceTypeHandler(DeviceDescription? dd)
    {
        var handler = OnDeviceType;
        handler?.Invoke(this, new DeviceTypeEventArgs(dd));
    }

    protected virtual void RaiseConnectedChanged(bool connected)
    {
        var handler = OnConnectedChanged;
        handler?.Invoke(this, connected);
    }

    protected virtual void RaiseNakHandler(NakCommandBlock.Nak error)
    {
        var handler = OnNak;
        handler?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     The raise status data handler.
    /// </summary>
    /// <param name="statusData">
    ///     The sender.
    /// </param>
    protected virtual void RaiseStatusDataChangedHandler(StatusData statusData)
    {
        var handler = OnStatusDataChanged;
        handler?.Invoke(this, new StatusDataEventArgs(statusData));
    }

    /// <summary>
    ///     The raise status data handler.
    /// </summary>
    /// <param name="statusData">
    ///     The sender.
    /// </param>
    protected virtual void RaiseStatusDataChangingHandler(StatusData statusData)
    {
        var handler = OnStatusDataChanging;
        handler?.Invoke(this, new StatusDataEventArgs(statusData));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusData"></param>
    protected virtual void RaiseStatusDataReceivedHandler(StatusData statusData)
    {
        var handler = OnStatusDataReceived;
        handler?.Invoke(this, new StatusDataEventArgs(statusData));
    }

    /// <summary>
    ///     The raise time data handler.
    /// </summary>
    /// <param name="senseReturn">
    ///     The sender.
    /// </param>
    /// <param name="timeCode">
    ///     The sender.
    /// </param>
    protected virtual void RaiseTimeDataReceivedHandler(SenseReturn senseReturn, TimeCode timeCode)
    {
        var handler = OnTimeDataReceived;
        handler?.Invoke(this, new TimeDataEventArgs(senseReturn, timeCode));
    }

    /// <summary>
    ///     The raise time data handler.
    /// </summary>
    /// <param name="senseReturn">
    ///     The sender.
    /// </param>
    /// <param name="timeCode">
    ///     The sender.
    /// </param>
    protected virtual void RaiseTimeDataChangedHandler(SenseReturn senseReturn, TimeCode timeCode)
    {
        var handler = OnTimeDataChanged;
        handler?.Invoke(this, new TimeDataEventArgs(senseReturn, timeCode));
    }

    /// <summary>
    ///     The raise time data handler.
    /// </summary>
    /// <param name="senseReturn">
    ///     The sender.
    /// </param>
    /// <param name="timeCode">
    ///     The sender.
    /// </param>
    protected virtual void RaiseTimeDataChangingHandler(SenseReturn senseReturn, TimeCode timeCode)
    {
        var handler = OnTimeDataChanging;
        handler?.Invoke(this, new TimeDataEventArgs(senseReturn, timeCode));
    }

    #endregion

    private readonly AutoResetEvent _workerThreadStopped = new(false);

    private readonly BackgroundWorker _serialReaderWorker = new() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };

    private readonly BackgroundWorker _idleWorker = new() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };

    private readonly AutoResetEvent _requestReady = new(false);

    private readonly AutoResetEvent _fireIdleCommand = new(false);

    private readonly System.Timers.Timer _idleTimer = new();

    private readonly Lock _lock = new();

    private bool? _connected = null;

    /// <summary>
    ///     Gets or sets the port name.
    /// </summary>
    public bool isConnected
    {
        get => _connected == true;

        set
        {
            if (value == _connected) return;

            _connected = value;
            RaiseConnectedChanged(_connected == true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Sony9PinMaster()
    {
        _serialReaderWorker.DoWork += SerialReader;
        _idleWorker.DoWork += IdleReader;

        _idleTimer.Enabled = false;
        _idleTimer.Interval = 5; // Idle timeout is 5ms
        _idleTimer.Elapsed += (sender, e) => _fireIdleCommand.Set();
        _idleTimer.AutoReset = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static async Task<NameValueCollection> DiscoverPorts(string[] serialPorts, ProtocolCallBack callback)
    {
        var activePorts = new NameValueCollection();

        foreach (var serialPort in serialPorts)
        {
            try
            {
                using var bvw75 = new Sony9PinMaster();

                bvw75.OnDeviceType += (sender, e) =>
                {
                    Debug.WriteLine($"DeviceType from {serialPort} {e.DeviceDescription.Model}");
                    activePorts.Add(serialPort, e.DeviceDescription.Model);
                };
                bvw75.OnNak += (sender, e) =>
                { 
                    Debug.WriteLine($"Nak from {serialPort}. Unplug and replug device");
                };

                await bvw75.Probe(serialPort, callback);

                bvw75.Dispose();
            }
            catch (Exception)
            {
            }
        }

        return activePorts;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public override async Task<bool> Open(string port, ProtocolCallBack callback)
    {
        // step 1. Open the serial port
        if (! await base.Open(port, callback))
            return false;

        // Start the ReaderWorker so that we can send a Command
        // down the Sony9Pin path and get a result back
        _serialReaderWorker.RunWorkerAsync(argument: this);
        // TODO: wait here until thread is fully ready

        _ = await SendAsync(new DeviceTypeRequest());

        _idleWorker.RunWorkerAsync(argument: this);
        _idleTimer.Enabled = true;

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public async Task<bool> Probe(string port, ProtocolCallBack callback)
    {
        Debug.WriteLine($"Start probing port {port}");

        // step 1. Open the serial port
        if (! await base.Open(port, callback))
            return false;

        _serialReaderWorker.RunWorkerAsync(argument: this);

        // step 2. Send a DeviceTypeRequest
        // response picked up by event handler in DiscoverPorts
        _ = await SendAsync(new DeviceTypeRequest());

        if (_serialReaderWorker is { WorkerSupportsCancellation: true })
            _serialReaderWorker.CancelAsync();

        base.Close();

        Debug.WriteLine($"Stop probing port {port}");

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override void Close()
    {
        // Let the this._queueReaderWorker know its time to finish
        // Doesn't matter is the tread is busy or not (ég is Open was not successful, the
        // QueueReaderWorker is not started and IsBusy will be false)
        if (_serialReaderWorker is { WorkerSupportsCancellation: true })
            _serialReaderWorker.CancelAsync();

        if (_idleWorker is { WorkerSupportsCancellation: true })
            _idleWorker.CancelAsync();

        base.Close();
    }

    /// <summary>
    /// </summary>
    /// <param name="res">
    ///     The response.
    /// </param>
    public void ProcessResponse(CommandBlock? res)
    {
        if (null == res)
            return;

        //Trace.WriteLineIf(TraceSwitch.TraceVerbose, "Response: " + response);

        switch (res.Cmd1)
        {
            case CommandFunction.Return:
                switch ((Return)res.Cmd2)
                {
                    case Return.Ack:
                        // Indication that last Cmd1 was alright
                        //RaiseAckHandler();
                        break;

                    case Return.Nak:
                        //The master must immediatly stop sending data when it receives a NAK +Error Data message.
                        //this.Enabled = false;

                        var bits = new BitArray(new int[] { res.Data[0] });
                        if (bits.Get((int)NakCommandBlock.Nak.ChecksumError))
                        {
                            Thread.Sleep(10);
                        }
                        if (bits.Get((int)NakCommandBlock.Nak.FrameError))
                        {
                            Thread.Sleep(10);
                        }
                        if (bits.Get((int)NakCommandBlock.Nak.OverrunError))
                        {
                            Thread.Sleep(10);
                        }
                        if (bits.Get((int)NakCommandBlock.Nak.ParityError))
                        {
                            Thread.Sleep(10);
                        }
                        if (bits.Get((int)NakCommandBlock.Nak.TimeOut))
                        {
                            Thread.Sleep(10);
                        }
                        if (bits.Get((int)NakCommandBlock.Nak.UndefinedError))
                        {
                        }
                        RaiseNakHandler((NakCommandBlock.Nak)res.Data[0]);
                        break;

                    case Return.DeviceType:
                        {
                            var deviceId = (ushort)(res.Data[0] << 8 | res.Data[1]);
                            if (!Device.Names.TryGetValue(deviceId, out var deviceDescription))
                            {
                                manufacturer = "Generic";
                                manufacturerShort = "Generic";
                                model = Convert.ToHexString(res.Data);
                            }
                            else
                            {
                                manufacturer = deviceDescription.Manufacturer;
                                manufacturerShort = deviceDescription.ManufacturerShort;
                                model = deviceDescription.Model;
                            }

                            RaiseDeviceTypeHandler(deviceDescription);
                        }
                        break;
                }

                break;

            case CommandFunction.SenseReturn:
                switch ((SenseReturn)res.Cmd2)
                {
                    case SenseReturn.Timer1Data:
                    case SenseReturn.Timer2Data:
                    case SenseReturn.LtcTimeData:
                    case SenseReturn.UserBitsLtcData:
                    case SenseReturn.VitcTimeData:
                    case SenseReturn.UserBitsVitcData:
                    case SenseReturn.GenTimeData:
                    case SenseReturn.GenUserBitsData:
                    case SenseReturn.CorrectedLtcTimeData:
                    case SenseReturn.HoldUbLtcData:
                    case SenseReturn.HoldVitcTimeData:
                    case SenseReturn.HoldUbVitcData:
                        var timeCode = new TimeCode(res.Data);
                        if (!timeCode.Equals(TimeCode))
                        {
                            lock (this)
                            {
                                TimeCode = timeCode;
                            }
                        }
                        break;

                    case SenseReturn.StatusData:
                        StatusData = new StatusData(res.Data);
                        break;
                }

                break;
        }

        Received(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void SerialReader(object? sender, DoWorkEventArgs e)
    {
        Debug.WriteLine("Sony9PinMaster Starting QueueReader");

        if (sender is not BackgroundWorker worker)
            throw new ArgumentNullException(nameof(sender));

        Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

        const int ResponseWiggleTime = 75;

        const int BufferSize = 32;
        Span<byte> buffer = stackalloc byte[BufferSize];
        int length = 0;

        _stream?.Flush();

        Stopwatch stopwatch = new();

        while (!worker.CancellationPending)
        {
            if (_stream is { CanRead: false })
                break;

            if (!_requestReady.WaitOne(1))
                continue;

            // Make sure we have an empty buffer
            length = 0; // Clear buffer

            stopwatch.Restart();

            try
            {
                // Read characters from the serialPort until we can create a 
                // complete and valid CommandBlock
                while (_stream is { CanRead: true })
                {
                    var b = _stream.ReadByte();
                    if (b == -1)
                        break; // No more data to read

                    if (length >= BufferSize)
                        throw new InvalidOperationException("Buffer overflow while waiting for CommandBlock");

                    buffer[length++] = (byte)b;

                    //                    Debug.WriteLine($"inputBuffer Len: {inputBuffer.Count()} in {stopwatch.ElapsedMilliseconds}");

                    if (!CommandBlock.TryParse(buffer[..length], out var result))
                        continue;

                    // OK, we have enough characters for a valid CommandBlock.
                    stopwatch.Stop();

                    if (stopwatch.ElapsedMilliseconds > SlaveResponseWithin)
                        Debug.WriteLine($"Slave Response Time longer than expected: {stopwatch.ElapsedMilliseconds} ms");

                    if (stopwatch.ElapsedMilliseconds > (SlaveResponseWithin + ResponseWiggleTime))
                        throw new TimeoutException($"Response took over 9ms. {stopwatch.ElapsedMilliseconds}");

                    //Debug.WriteLine($"Slave Response within: {stopwatch.ElapsedMilliseconds} ms");
                    //Debug.Assert(0 == _stream.BytesToRead, "serial bytes remaining is not zero");

                    ProcessResponse(result!);

                    isConnected = true;

                    // We are done here, break back into the main loop to 
                    // try to take another CommandBlack
                    break;
                }
            }
            catch (TimeoutException)
            {
                stopwatch.Stop();
                Debug.WriteLine($"TimeoutException, Slave Response within: {stopwatch.ElapsedMilliseconds} ms");

                isConnected = false;

                Received(null!); // return error object

                _stream?.Flush();
            }
            catch (Exception)
            {
                break; // Leave after this error
            }
            finally
            {
                _idleTimer.Start();
            }
        }

        e.Cancel = true;

        _workerThreadStopped.Set(); // signal that worker is done

        // Indicate here that we have been disconnected
        isConnected = false;

        Debug.WriteLine("Sony9PinMaster Stopped QueueReader");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void IdleReader(object? sender, DoWorkEventArgs e)
    {
        Debug.WriteLine("Sony9PinMaster Starting IdleReader");

        var _currentTimeSenseOrStatusSense = 0;

        if (sender is not BackgroundWorker worker)
            throw new ArgumentNullException(nameof(sender));

        while (!worker.CancellationPending)
        {
            // After last good CommandBlock is received, await for idle.
            // Then send idle command

            try
            {
                if (!_fireIdleCommand.WaitOne(1))
                    continue;

                Debug.Assert(_serialReaderWorker.IsBusy, "SerialReaderWorker is busy");
                Debug.Assert(_serialReaderWorker.CancellationPending == false, "SerialReaderWorker CancellationPending");

                switch (_currentTimeSenseOrStatusSense)
                {
                    case 0:
                        {
                            _ = SendAsync(new StatusSense()).Result;
                            break;
                        }
                    case 1:
                        {
                            _ = SendAsync(new CurrentTimeSense(TimeSenseRequest.LtcTime)).Result;
                            break;
                        }
                }

                _currentTimeSenseOrStatusSense++;
                if (_currentTimeSenseOrStatusSense > 1)
                    _currentTimeSenseOrStatusSense = 0;
            }
            catch (AggregateException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        Debug.WriteLine("Sony9PinMaster Stopped IdleReader");
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Send(CommandBlock req)
    {
        _idleTimer.Stop();
        _requestReady.Set();

        var bytes = req.ToBytes();
        lock (_lock)
        {
            if (_stream is { CanWrite: true })
                _stream.Write(bytes, 0, bytes.Length);
        }
    }

}
