using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

namespace dotNetSony9Pin.Odetics;

public class OdeticsMaster : Sony9PinMaster
{
    public override CommandBlock Play() { return new Play(); }
}
