using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

public enum PlayBackType : byte
{
    Play = 0x0,
    Jog = 0x1,
    Shuttle = 0x2,
    Var = 0x3,
}

/// <summary>
/// 
/// </summary>
public class BMDPlay : CommandBlock
{
    /// <summary>
    /// </summary>
    public BMDPlay(short speed, bool loop, bool singleClip, PlayBackType playBackType, byte scroll)
    {
        var data = new byte[5];

        // 2 Bytes 16bit big endian signed integer, which
        // is the speed to play at, where a value of 100 = 1.0x
        data[0] = (byte)speed;
        data[1] = (byte)(speed >> 8);

        // playback flags bitfield, where bit 0 = Loop and bit 1 = SingleClip
        data[2] = 0;
        if (loop)
            data[2] |= 1 << 0;
        if (singleClip)
            data[2] |= 1 << 1;

        // playback type
        // 0 = Play
        // 1 = Jog
        // 2 = Shuttle
        // 3 = Var
        data[3] = (byte)playBackType;
        // which is the scroll boolean flag, where 0 evaluates as
        // false and all other values evaluate as true.
        data[4] = scroll;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.rrrReturn, data.Length);
        Cmd2 = (byte)BlackmagicExtensions.BMDPlay;
        Data = data;
    }
}
