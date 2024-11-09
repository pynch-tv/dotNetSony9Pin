using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

internal class BMDSeekToTimelinePosition : CommandBlock
{
    public BMDSeekToTimelinePosition(ushort position)
    {
        var data = new byte[2];

        // 16-bit big endian fractional position [0..65535]
        data[0] = (byte)position;
        data[1] = (byte)(position >> 8);

        Cmd1DataCount = ToCmd1DataCount((Cmd1)0x8, data.Length);
        Cmd2 = (byte)BlackmagicExtensions.BMDSeekToTimelinePosition;
        Data = data;
    }
}
