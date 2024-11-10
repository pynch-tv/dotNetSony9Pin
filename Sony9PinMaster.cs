using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using lathoub.dotNetSony9Pin.Pattern;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.Return;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.SenseRequest;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.SenseReturn;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.StatusData;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.SystemControl;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TimeSenseRequest;

namespace lathoub.dotNetSony9Pin;

public class Sony9PinMaster : Sony9PinBase
{
    public string Model { get; internal set; } = "";

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

    public TimeCode TimeCode
    {
        get => _timeCode;
    }

    #region Events and EventHandlers

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<DeviceTypeEventArgs>? DeviceType;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<ConnectedEventArgs>? ConnectedChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? Nak;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? StatusDataReceived;

    /// <summary>
    ///     The status data.
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? StatusDataChanged;

    /// <summary>
    ///     The status data.
    /// </summary>
    public event EventHandler<StatusDataEventArgs>? StatusDataChanging;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? TimeDataReceived;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? TimeDataChanged;

    /// <summary>
    ///     The time data.
    /// </summary>
    public event EventHandler<TimeDataEventArgs>? TimeDataChanging;

    /// <summary>
    ///     The raise device type handler.
    /// </summary>
    /// <param name="deviceName">
    ///     The sender.
    /// </param>
    protected virtual void RaiseDeviceTypeHandler(string deviceName)
    {
        var handler = DeviceType;
        handler?.Invoke(this, new DeviceTypeEventArgs(deviceName));
    }

    protected virtual void RaiseConnectedChanged(bool connected)
    {
        var handler = ConnectedChanged;
        handler?.Invoke(this, new ConnectedEventArgs(connected));
    }

    protected virtual void RaiseNakHandler(NakCommandBlock.Nak error)
    {
        var handler = Nak;
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
        var handler = StatusDataChanged;
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
        var handler = StatusDataChanging;
        handler?.Invoke(this, new StatusDataEventArgs(statusData));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusData"></param>
    protected virtual void RaiseStatusDataReceivedHandler(StatusData statusData)
    {
        var handler = StatusDataReceived;
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
        var handler = TimeDataReceived;
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
        var handler = TimeDataChanged;
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
        var handler = TimeDataChanging;
        handler?.Invoke(this, new TimeDataEventArgs(senseReturn, timeCode));
    }

    #endregion

    private readonly AutoResetEvent _workerThreadStopped = new(false);

    private readonly BackgroundWorker _serialReaderWorker = new() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
    private readonly BackgroundWorker _idleWorker = new() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };

    private readonly AutoResetEvent _requestReady = new(false);

    private readonly AutoResetEvent _fireIdleCommand = new(false);

    private readonly System.Timers.Timer _idleTimer = new();

    private readonly object _lock = new();

    private bool _connected;

    /// <summary>
    ///     Gets or sets the port name.
    /// </summary>
    public bool IsConnected
    {
        get => _connected;

        set
        {
            if (value == _connected) return;

            _connected = value;
            RaiseConnectedChanged(_connected);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Sony9PinMaster()
    {
        _serialReaderWorker.DoWork += SerialReader;
        _idleWorker.DoWork += IdleReader;

        _idleTimer.Interval = 5; // Idle timeout is 5ms
        _idleTimer.Elapsed += (sender, e) => _fireIdleCommand.Set();
        _idleTimer.AutoReset = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static NameValueCollection DiscoverPorts()
    {
        var activePorts = new NameValueCollection();

        foreach (var serialPort in SerialPort.GetPortNames())
        {
            using var bvw75 = new Sony9PinMaster();

            if (!bvw75.Open(serialPort))
                continue;

            try
            {
                try
                {
                    var dtr = bvw75.SendAsync(new DeviceTypeRequest());
                    if (null == dtr.Result)
                    {
                        Debug.WriteLine("No response from device");
                        continue;
                    }

                    var deviceName = dtr.Result.ToString();

                    activePorts.Add(serialPort, deviceName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                bvw75.Close();
            }
        }

        return activePorts;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public override bool Open(string port)
    {
        if (!base.Open(port))
            return false;

        _serialReaderWorker.RunWorkerAsync();
        _idleWorker.RunWorkerAsync();

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

        // Wait here for worker thread to signal its done
        if (_serialReaderWorker.IsBusy || _idleWorker.IsBusy)
            if (!_workerThreadStopped.WaitOne()) // will block until _workerThreadStopped.Set() call made at end of worker thread
                Debug.WriteLine("Could not stop serial master thread");

        base.Close();
    }

    /// <summary>
    /// </summary>
    /// <param name="res">
    ///     The response.
    /// </param>
    public void ProcessResponse(CommandBlock res)
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
                            string device;
                            if (!Device.Names.TryGetValue(deviceId, out var deviceDescription))
                                device = BitConverter.ToString(res.Data).Replace("-", string.Empty);
                            else
                                device = deviceDescription.Model;
                            Model = device ?? "Unknown";

                            RaiseDeviceTypeHandler(Model);
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
                        if (!timeCode.Equals(_timeCode))
                        {
                            RaiseTimeDataChangingHandler((SenseReturn)res.Cmd2, _timeCode);
                            lock (this)
                            {
                                _timeCode = timeCode;
                            }
                            RaiseTimeDataChangedHandler((SenseReturn)res.Cmd2, _timeCode);
                        }
                        RaiseTimeDataReceivedHandler((SenseReturn)res.Cmd2, _timeCode);
                        break;

                    case SenseReturn.StatusData:
                        StatusData = new StatusData(res.Data);
                        break;
                }

                break;
        }
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

        // Make sure we have an empty buffer
        List<byte>? InputBuffer = new(32);

        while (!worker.CancellationPending)
        {
            if (!_serialPort.IsOpen)
                break;

            //
            if (!_requestReady.WaitOne(1))
                continue;

            // Flush serial buffers, both in and out
            lock (_lock)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
            }
            // Make sure we have an empty buffer
            InputBuffer.Clear();

            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                // Read characters from the serialPort until we can create a 
                // complete and valid CommandBlock
                while (_serialPort.IsOpen)
                {
                    var b = _serialPort.ReadByte();
                    if (b == -1)
                        break; // No more data to read
                    InputBuffer.Add((byte)b);

                    if (stopwatch.ElapsedMilliseconds > 9)
                        throw new TimeoutException($"Response took over 9ms. {stopwatch.ElapsedMilliseconds}");

                    if (!CommandBlock.TryParse(InputBuffer, out CommandBlock? res))
                        continue;
                    if (null == res) break;

                    // OK, we have enough characters for a valid CommandBlock.
                    stopwatch.Stop();
                    Debug.WriteLine($"Response only in: {stopwatch.ElapsedMilliseconds} ms");
                    Debug.Assert(0 == _serialPort.BytesToRead, "serial bytes remaining is not zero");

                    var btr = _serialPort.BytesToRead;
                    if (btr > 0)
                    {
                        Debug.WriteLine($"{btr} bytes remaining to be read??");
                        for (int i = 0; i < btr; i++)
                        {
                            Debug.WriteLine($"0x{_serialPort.ReadByte():X}");
                        }
                    }

                    IsConnected = true;

                    Received(res);

                    // We are done here, break back into the main loop to 
                    // try to take another CommandBlack
                    break;
                }
            }
            catch (TimeoutException ex)
            {
                // Oei - a character couldn't be read within the time given.
                Debug.WriteLine(ex.Message);

                IsConnected = false;

                Received(null); // return error object
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
        IsConnected = false;

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

        int _currentTimeSenseOrStatusSense = 0;

        if (sender is not BackgroundWorker worker)
            throw new ArgumentNullException(nameof(sender));

        while (!worker.CancellationPending)
        {
            // After last good CommandBlock is received, await for idle.
            // Then send idle command

            if (!_fireIdleCommand.WaitOne(1))
                continue;

            if (_currentTimeSenseOrStatusSense == 0)
            {
                _ = SendAsync(new StatusSense()).Result;
                Debug.WriteLine($"==============================================================");
            }
            else if (_currentTimeSenseOrStatusSense == 1)
            {
                _ = SendAsync(new CurrentTimeSense(TimeSenseRequest.LtcTime)).Result;
                Debug.WriteLine($"==============================================================");
            }

            _currentTimeSenseOrStatusSense++;
            if (_currentTimeSenseOrStatusSense > 1)
                _currentTimeSenseOrStatusSense = 0;
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
            _serialPort.Write(bytes, 0, bytes.Length);
        }
    }

}
