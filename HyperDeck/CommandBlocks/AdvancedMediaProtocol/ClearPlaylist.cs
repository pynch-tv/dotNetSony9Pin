using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

public class ClearPlaylist : CommandBlock
{
    public ClearPlaylist()
    {
        Cmd1 = (Cmd1)0x8;
        Cmd2 = (byte)AdvancedMediaProtocol.ClearPlaylist;
    }
}
