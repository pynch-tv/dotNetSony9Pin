// See https://aka.ms/new-console-template for more information

using dotNetSony9Pin;
using System.IO.Ports;
using System.Net.Sockets;
using System.Collections.Specialized;

SerialPort serialPort = new();

TcpClient socket = new(); 

Stream OpenSerialStream(string port)
{
    serialPort.PortName = port;
    serialPort.BaudRate = 38400;
    serialPort.DataBits = 8;
    serialPort.StopBits = StopBits.One;
    serialPort.Parity = Parity.Odd;
    serialPort.Handshake = Handshake.None;
    serialPort.DtrEnable = true;
    serialPort.RtsEnable = true;

    serialPort.ReadTimeout = 250;

    serialPort.Open();

    return serialPort.BaseStream;
}

Stream OpenSocketStream(string hostPort)
{
    var parts = hostPort.Split(':');
    var host = parts[0];
    var port = int.Parse(parts[1]); // defauls to 9096

    try
    {
        socket = new TcpClient(host, port);
    }
    catch (SocketException ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }

    return socket.GetStream();
}

async Task<NameValueCollection> Discover()
{
    return await Sony9PinMaster.DiscoverPorts(SerialPort.GetPortNames(), OpenSerialStream);
}

/*
var ports = await Discover();
if (ports.Count == 0)
{
    Console.WriteLine("No devices found.");
    return;
}
*/

var master = new Sony9PinMaster();

master.OnStatusDataChanged += (sender, e) =>
{
    Console.WriteLine($"Status Data Changed: {e.StatusData}");
};

master.OnTimeDataChanged += (sender, e) =>
{
    Console.WriteLine($"Time Data Changed: {e.TimeCode}");
};

if (!await master.Open("100.92.235.46:9096", OpenSocketStream))
{
    Console.WriteLine("Failed to open port.");
    return;
}

try
{
    //var t1 = await master.SendAsync(new DeviceTypeRequest());
    //var t2 = await master.SendAsync(new DeviceTypeRequest());
    //var t3 = await master.SendAsync(new DeviceTypeRequest());

    //var t4 = await master.SendAsync(new ListNextID());

    //  var a2 = await master.SendAsync(new BMDPlay(1, false, false, PlayBackType.Play, 0));
    //  Debug.WriteLine(t2);

//    var ee = await master.SendAsync(new BMDClip());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.Escape)
            break;
    }

    var random = new Random();
    var ms = random.Next(3, 750);
    Thread.Sleep(ms);

    //try
    //{
    //    var r = master.SendAsync(new DeviceTypeRequest()).Result;
    //    Debug.WriteLine(master.ParseResponse(r));
    //    Debug.WriteLine($"==============================================================");
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    Thread.Sleep(1);
}

master.Close();
