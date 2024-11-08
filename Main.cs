// See https://aka.ms/new-console-template for more information
using lathoub;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var hyperDeck = new HyperDeck();

hyperDeck.Connecting += (sender, e) => Debug.WriteLine("Connecting");
hyperDeck.Connected += (sender, e) => Debug.WriteLine("Connected");
hyperDeck.Disconnecting += (sender, e) => Debug.WriteLine("Disconnecting");
hyperDeck.Disconnected += (sender, e) => Debug.WriteLine("Disconnected");

hyperDeck.AsyncResponse += (sender, e) => Debug.WriteLine( $"Async Msg: {e.ToString()}");

hyperDeck.Connect("192.168.0.196", 9993);

var result1 = hyperDeck.SendAsync(new Request("ping")).Result;

var result2 = hyperDeck.SendAsync(new Request("ping")).Result;

var result3 = hyperDeck.SendAsync(new Request("ping")).Result;

var result4 = hyperDeck.SendAsync(new Request("ping")).Result;

var result5 = hyperDeck.SendAsync(new Request("ping")).Result;

var result6 = hyperDeck.SendAsync(new Request("ping")).Result;

hyperDeck.Disconnect();

Console.WriteLine("Done");
