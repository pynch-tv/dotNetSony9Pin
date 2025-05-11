using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using Pynch.Tools;

namespace dotNetSony9Pin.Pattern;

public delegate Stream ProtocolCallBack(string port);

public abstract class Sony9PinBase : RequestResponsePump<CommandBlock, CommandBlock>, IDisposable
{
    protected Stream? _stream;

    private bool _disposed;

    /// <summary>
    /// Time in milliseconds
    /// </summary>
    protected int SlaveResponseWithin { get; } = 9;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <param name="callback"></param>
    public virtual async Task<bool> Open(string port, ProtocolCallBack callback)
    {
        _stream = callback(port);

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual void Close()
    {
        _stream?.Dispose();
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
