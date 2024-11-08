using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lathoub;

public abstract class NetworkPump<T, U> : RequestResponsePump<T, U>
{
    protected readonly TcpClient _tcpClient = new();

    private NetworkStream? _networkStream;

    public IPAddress Host { get; private set; } = IPAddress.Any;

    public ushort Port { get; private set; } = 9993;

    #region Events

    public event EventHandler? Connecting;

    public event EventHandler? Connected;

    public event EventHandler? Disconnecting;

    public event EventHandler? Disconnected;

    protected void RaiseConnecting()
    {
        var handler = Connecting;
        handler?.Invoke(this, EventArgs.Empty);
    }

    protected void RaiseConnected()
    {
        var handler = Connected;
        handler?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void RaiseDisconnecting()
    {
        var handler = Disconnecting;
        handler?.Invoke(this, EventArgs.Empty);
    }

    protected void RaiseDisconnected()
    {
        var handler = Disconnected;
        handler?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public virtual bool Connect(string host, ushort port)
    {
        RaiseConnecting();

        this.Host = IPAddress.Parse(host);
        this.Port = port;

        var ca = _tcpClient.ConnectAsync(host, port);
        var completedTask = Task.WhenAny(ca, Task.Delay(1000));

        if (!completedTask.IsCompleted) return false;
        if (!_tcpClient.Connected) return false;

        _networkStream = _tcpClient.GetStream();
        if (null == _networkStream) return false;

        Start();

        RaiseConnected();

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual void Disconnect()
    {
        RaiseDisconnecting();

        Stop();

        RaiseDisconnected();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start()
    {
        var buffer = new byte[4048];
        _ = _networkStream?.BeginRead(buffer, 0, buffer.Length, OnReadComplete, buffer);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Stop()
    {
        _tcpClient.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iar"></param>
    protected void OnReadComplete(IAsyncResult iar)
    {
        try
        {
            var buffer = (byte[])iar.AsyncState!;
            var bytesRead = _networkStream!.EndRead(iar);

            DataArrival(buffer, bytesRead);

            _networkStream?.BeginRead(buffer, 0, buffer.Length, OnReadComplete, buffer);
        }
        catch (Exception)
        {
        }
    }

    protected abstract void DataArrival(byte[] buffer, int bytesRead);

    /// <summary>
    /// 
    /// </summary>
    protected override void Send(T req)
    {
        Debug.WriteLine($"Send: {req}");

        var buffer = Encoding.UTF8.GetBytes(req.ToString());
        if (null != _networkStream)
            _networkStream.WriteAsync(buffer);
    }
}
