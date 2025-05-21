using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

namespace dotNetSony9Pin.EVS;

public class EVSMaster : Sony9PinMaster
{
    public override CommandBlock Play() { return new Play(); }
}
