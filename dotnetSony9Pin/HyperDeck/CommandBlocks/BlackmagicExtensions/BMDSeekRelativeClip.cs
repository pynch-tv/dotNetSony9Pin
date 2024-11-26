using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

public class BMDSeekRelativeClip : CommandBlock
{
    public BMDSeekRelativeClip(byte numberOfClips)
    {
        var data = new byte[1];

        // number of clips to skip(negative for backwards).
        data[0] = numberOfClips;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.rrrReturn, data.Length);
        Cmd2 = (byte)BlackmagicExtensions.BMDSeekRelativeClip;
        Data = data;
    }
}
