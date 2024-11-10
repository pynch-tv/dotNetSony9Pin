using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

public class ClearPlaylist : CommandBlock
{
    public ClearPlaylist()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)AdvancedMediaProtocol.ClearPlaylist;
    }
}
