using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

public enum StopMode : byte
{
    PlayOff = 0x0,
    FreezeOnLastFrame = 0x1,
    FreezeOnNextClip = 0x2,
    ShowBlack = 0x3,
}

internal class SetStopMode : CommandBlock
{
    /// <summary>
    /// 0 = Off
    /// 1 = Freeze on last frame
    /// 2 = Freeze on next clip
    /// 3 = Show black
    /// </summary>
    /// <param name="mode"></param>
    public SetStopMode(StopMode mode)
    {
        var data = new byte[1];
        data[0] = (byte)mode;

        Cmd1 = (Cmd1)0x8;
        Cmd2 = (byte)AdvancedMediaProtocol.SetStopMode;
        Data = data;
    }
}
