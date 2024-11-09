using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

public class BMDClip : CommandBlock
{
    /// <summary>
    /// </summary>
    public BMDClip()
    {
        Cmd1 = (Cmd1)0x8;
        Cmd2 = (byte)BlackmagicExtensions.BMDClip;
    }
}
