using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

public class BMDClip : CommandBlock
{
    /// <summary>
    /// </summary>
    public BMDClip()
    {
        Cmd1 = CommandFunction.rrrReturn;
        Cmd2 = (byte)BlackmagicExtensions.BMDClip;
    }
}
