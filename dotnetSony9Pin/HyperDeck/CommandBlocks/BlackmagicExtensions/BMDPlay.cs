using System.Runtime.InteropServices.JavaScript;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

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
    private static byte[] ConvertToBigEndianInt16(short value)
    {
        var speedAsShort = Convert.ToInt16(value.ToString(), 16);

        var bytes = BitConverter.GetBytes(speedAsShort);
        Array.Reverse(bytes);

        return bytes;
    }

    /// <summary>
    /// 2 Bytes 16bit big endian signed integer, which  is the speed to play at, 
    ///    where a value of 100 = 1.0x
    /// 1 Byte unsigned integer, which is the playback flags bitfield, where
    ///     bit 0 = Loop
    ///     bit 1 = SingleClip
    /// 1 Byte unsigned integer, which is the playback type, where
    ///     0 = Play
    ///     1 = Jog
    ///     2 = Shuttle
    ///     3 = Var
    /// 1 Byte unsigned integer, which is the scroll boolean flag, 
    ///     where 0 evaluates as false and all other values evaluate as true
    /// </summary>
    public BMDPlay(short speed, bool loop, bool singleClip, PlayBackType playBackType, byte scroll)
    {
        var data = new byte[5];

        // 2 Bytes 16bit big endian signed integer, which
        // is the speed to play at, where a value of 100 = 1.0x
        var speedInBytes = ConvertToBigEndianInt16(speed);
        data[0] = speedInBytes[0];
        data[1] = speedInBytes[1];

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
