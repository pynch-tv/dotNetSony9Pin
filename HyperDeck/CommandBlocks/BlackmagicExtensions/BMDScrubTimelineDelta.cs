using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

public class BMDScrubTimelineDelta : CommandBlock
{
    public BMDScrubTimelineDelta(byte whence, int deltaToScrub, byte deltaSign, byte unitOfTimeScrub)
    {
        var data = new byte[7];

        data[0] = whence;

        data[1] = (byte)deltaToScrub;
        data[2] = (byte)(deltaToScrub >> 8);
        data[3] = (byte)(deltaToScrub >> 16);
        data[4] = (byte)(deltaToScrub >> 24);

        data[5] = deltaSign;

        data[6] = unitOfTimeScrub;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.rrrReturn, data.Length);
        Cmd2 = (byte)BlackmagicExtensions.BMDScrubTimelineDelta;
        Data = data;
    }

}
