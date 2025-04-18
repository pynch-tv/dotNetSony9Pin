// See https://aka.ms/new-console-template for more information

using dotNetSony9Pin;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.SystemControl;
using System.IO.Ports;
using System.Net.Sockets;
using dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;
using dotNetSony9Pin.Odetics.CommandBlocks.xxxRequest;

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

//master.Open("192.168.0.13:9096", OpenSocketStream);
master.Open("COM5", OpenSerialStream);

try
{
    var t1 = await master.SendAsync(new DeviceTypeRequest());
    var t2 = await master.SendAsync(new DeviceTypeRequest());
    var t3 = await master.SendAsync(new DeviceTypeRequest());

  //      _ = master.SendAsync(new ListNextID(0)).Result;

      var a2 = await master.SendAsync(new BMDPlay(1, false, false, PlayBackType.Play, 0));
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
