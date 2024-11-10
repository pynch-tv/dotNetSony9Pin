using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class AppendPreset : CommandBlock
{
    /// <summary>
    /// 2 Bytes for the length N of the clip name
    /// N Bytes for each character of the clip name
    /// 4 Byte in point timecode (format is FFSSMMHH)
    /// 4 Byte out point timecode (format is FFSSMMHH)
    /// </summary>
    public AppendPreset()
    {
        var data = new byte[5];

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.PresetSelectControl, data.Length);
        Cmd2 = (byte)AdvancedMediaProtocol.AppendPreset;
        Data = data;
    }
}
