// See https://aka.ms/new-console-template for more information
using dotNetSony9Pin;
using dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.SystemControl;
using System.IO.Ports;
using System.Net.Sockets;

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

    socket = new TcpClient(host, port);

    return socket.GetStream();
}

//var ports = Sony9PinMaster.DiscoverPorts();
//if (ports.Count == 0)
//{
//    Console.WriteLine("No ports found");
//    return;
//}

//var firstPort = ports[0];

var master = new Sony9PinMaster();

master.Open("192.168.0.13:9096", OpenSocketStream);

try
{
    _ = master.SendAsync(new DeviceTypeRequest()).Result;

    //_ = master.SendAsync(new ListNextID(0)).Result;
    //Debug.WriteLine(Sony9PinMaster.ParseResponse(t2));

    //_ = master.SendAsync(new BMDPlay(1, false, false, PlayBackType.Play, 0)).Result;
    //Debug.WriteLine(t2);

    _ = master.SendAsync(new BMDClip()).Result;

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
