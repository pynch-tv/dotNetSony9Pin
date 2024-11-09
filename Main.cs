// See https://aka.ms/new-console-template for more information
using lathoub;
using Pynch.Sony9Pin.Core.Sony9Pin.CommandBlocks.SystemControl;
using System.Diagnostics;

var master = new Sony9PinMaster();

master.Connect("COM3");

try
{
    var tt = master.SendAsync(new DeviceTypeRequest()).Result;
    Debug.WriteLine(master.ParseResponse(tt));
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

    try
    {
        var r = master.SendAsync(new DeviceTypeRequest()).Result;
        Debug.WriteLine(master.ParseResponse(r));
        Debug.WriteLine($"==============================================================");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    Thread.Sleep(1);
}

master.Disconnect();
