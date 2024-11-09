using System.IO.Ports;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.Pattern;

public abstract class Sony9PinBase : RequestResponsePump<CommandBlock, CommandBlock>, IDisposable
{
    protected readonly SerialPort _serialPort = new();

    public string Port => _serialPort.PortName;

    private bool _disposed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public virtual bool Open(string port)
    {
        _serialPort.PortName = port;
        _serialPort.BaudRate = 38400;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.Parity = Parity.Odd;
        _serialPort.Handshake = Handshake.None;
        _serialPort.DtrEnable = true;
        _serialPort.RtsEnable = true;
        _serialPort.ReadTimeout = 250; // Sony9Pin specs indicate 9ms

        _serialPort.Open();

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual void Close()
    {
        _serialPort.Close();
    }

    /// <summary>
    ///     Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">
    ///     true if managed resources should be disposed; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            try
            {
                if (disposing)
                {
                    // Release the managed resources
                    Close();
                }

                // Release the native unmanaged resources here.
                // NOP                        
            }
            finally
            {
                // Call Dispose on the base class, if any
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        Dispose(true);

        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC.SupressFinalize to
        // take this object off the finalization queue 
        // and prevent finalization code for this object
        // from executing a second time.
        GC.SuppressFinalize(this);
    }
}
