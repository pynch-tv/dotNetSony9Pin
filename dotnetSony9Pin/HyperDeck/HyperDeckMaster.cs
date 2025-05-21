using dotNetSony9Pin.Sony9Pin.CommandBlocks;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

namespace dotNetSony9Pin.HyperDeck;

public class HyperDeckMaster : Sony9PinMaster
{
    public CommandBlock Play()
    {
        return new Play();
    }
    public CommandBlock Stop()
    {
        return new Stop();
    }
}
