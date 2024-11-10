using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class SetPlaybackLoop : CommandBlock
{
    /// <summary>
    /// Bit 0 loop mode enable, 0=false 1=true
    /// Bit 1 is single clip/timeline
    /// 0=single clip
    /// 1=timeline
    /// </summary>
    /// <param name="loop"></param>
    public SetPlaybackLoop(bool loop, bool singleClip)
    {
        var data = new byte[1];
        data[0] = 0;
        if (loop)
            data[0] |= 1 << 0;
        if (singleClip)
            data[0] |= 1 << 1;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.PresetSelectControl, data.Length);
        Cmd2 = (byte)AdvancedMediaProtocol.SetPlaybackLoop;
        Data = data;
    }
}
