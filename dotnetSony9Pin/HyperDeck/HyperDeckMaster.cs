using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;
using dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

namespace dotNetSony9Pin.HyperDeck;

public class HyperDeckMaster : Sony9PinMaster
{
    public override CommandBlock Play() { return new Play(); }
}
