// See https://aka.ms/new-console-template for more information
using lathoub.dotNetSony9Pin;
using lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;
using System.Diagnostics;

var master = new Sony9PinMaster();

master.Connect("COM3");

try
{
    //var tt = master.SendAsync(new DeviceTypeRequest()).Result;
    //Debug.WriteLine(master.ParseResponse(tt));
    //Debug.WriteLine($"==============================================================");


    var t2 = master.SendAsync(new BMDPlay(1, false, false, PlayBackType.Play, 0)).Result;
    Debug.WriteLine(master.ParseResponse(t2));
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

master.Disconnect();
