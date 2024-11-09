// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.Diagnostics;
using lathoub.dotNetSony9Pin;
using lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

var ports = Sony9PinMaster.DiscoverPorts();
if (ports.Count == 0)
{
    Console.WriteLine("No ports found");
    return;
}

var firstPort = ports[0];

var master = new Sony9PinMaster();

master.Open(firstPort);

try
{
    //var tt = master.SendAsync(new DeviceTypeRequest()).Result;
    //Debug.WriteLine(master.ParseResponse(tt));
    //Debug.WriteLine($"==============================================================");


    var t2 = master.SendAsync(new BMDPlay(1, false, false, PlayBackType.Play, 0)).Result;
    Debug.WriteLine(Sony9PinMaster.ParseResponse(t2));
    Debug.WriteLine($"==============================================================");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

while (true) {
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
