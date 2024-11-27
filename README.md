# dotNetSony9Pin

A .Net Core (C#) library to control a video recorder using the Sony 9-Pin protocol. The Odetics protocol is a superset of the Sony 9pin protocol.

The Odetics extension of the Sony9Pin protocol is also included in this repo. It is a superset of the popular Sony 9-Pin VTR control protocol. Most of the added functionnalities are about “Clip” management by which the user can enqueue “Load Clip”/“Load Next Clip” commands.

## C# Usage

### Master
This implementation of the Sony9PinMaster automatically requests TimeCode and StatusData when the command queue is empty, the user does not need to program this her/himself. Commands issued using the Command method will be put on top of the queue. 

```
var master = new Sony9PinMaster();
master.Open("COM3");

try
{
    var deviceType = master.SendAsync(new DeviceTypeRequest()).Result;
    var play = master.SendAsync(new Play()).Result;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

```
