using System.Text;

namespace lathoub;

internal class HyperDeck : NetworkPump<Request, Response>
{
    private string _buffer = string.Empty;

    #region Events

    public event EventHandler<Response> AsyncResponse;

    private event EventHandler<Response> SyncResponse;

    private void RaiseAsyncResponse(Response res)
    {
        var handler = AsyncResponse;
        handler?.Invoke(this, res);
    }


    private void RaiseSyncResponse(Response res)
    {
        var handler = SyncResponse;
        handler?.Invoke(this, res);
    }

    #endregion

    public bool IsConnected => _tcpClient?.Connected ?? false;

    public string Id { get; private set; } = "";

    public string Name { get; set; } = "";

    public string Model { get; internal set; } = "";

    public string Serial { get; internal set; } = "";

    public string SoftwareVersion { get; internal set; } = "";

    public string HardwareVersion { get; internal set; } = "";

    public Dictionary<int, SlotInfo> SlotInfos { get; } = [];

    public int SlotCount => SlotInfos.Count;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override bool Connect(string host, ushort port)
    {
        this.SyncResponse += OnSyncResponse;

        void handler(object sender, EventArgs e)
        {
            this.Connected -= handler;

            var deviceInfo = SendAsync(new Request("device info")).Result;
            if (deviceInfo == null)
                return;

            Name = deviceInfo.Parameters["name"] ?? "";
            Id = deviceInfo.Parameters["name"] ?? "";
            Serial = deviceInfo.Parameters["unique id"] ?? ""; ;
            Model = deviceInfo.Parameters["model"] ?? ""; ;
            SoftwareVersion = deviceInfo.Parameters["software version"] ?? ""; ;
            HardwareVersion = deviceInfo.Parameters["protocol version"] ?? ""; ;

            var slotCount = int.Parse(deviceInfo.Parameters["slot count"] ?? "0");
            for (var i = 1; i <= slotCount; i++)
            {
                var slotInfo = SendAsync(new Request($"slot info: slot id: {i}")).Result;
                if (slotInfo != null && slotInfo.Code == ResponseCode.OkSlotInfo)
                {
                    var si = SlotInfo.FromSlotInfo(slotInfo.Parameters);
                    SlotInfos.Add(si.Id, si);
                }
            }
        };
        this.Connected += handler;

        return base.Connect(host, port);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override void Disconnect()
    {
        base.Disconnect();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override void Stop()
    {
        this.SyncResponse -= OnSyncResponse;

        base.Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSyncResponse(object? sender, Response res)
    {
        Received(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bytesRead"></param>
    protected override void DataArrival(byte[] bytes, int bytesRead)
    {
        // Add to the end of the string _buffer
        _buffer += Encoding.UTF8.GetString(bytes, 0, bytesRead);

        Parse();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Parse()
    {
        while (true)
        {
            var pos = _buffer.IndexOf("\r\n");
            if (pos < 0)
                break; // not enough data

            string message;

            var s = _buffer[..pos];

            if (s.EndsWith(":"))
            {
                // MultiLine end with \r\n\r\n - find it
                var lastIndex = _buffer.IndexOf("\r\n\r\n");
                if (lastIndex < 0)
                    break; // not enough data

                // take what we need
                message = _buffer[..(lastIndex + 4)];
                // remove what we used from the start of the buffer. Maybe there is more
                _buffer = _buffer[(lastIndex + 4)..];
            }
            else
            {
                message = _buffer[..pos];
                _buffer = _buffer[(pos + 2)..];
            }

            // extract response from bytes
            if (!Response.TryParse(message, out var response))
            {
                break;
            }

            if (Enumerable.Range(500, 599).Contains((int)response.Code))
                RaiseAsyncResponse(response);
            else
                RaiseSyncResponse(response);

            if (_buffer.Length == 0)
                break;
        }
    }


}
