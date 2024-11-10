using lathoub.dotNetSony9Pin;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace Pynch.Sony9Pin.Core.Odetics.CommandBlocks.PresetSelectControl;

/// <summary>
/// </summary>
public class OutPreset : CommandBlock
{
    /// <summary>
    /// </summary>
    public OutPreset()
    {
        Cmd1 = CommandFunction.PresetSelectControl;
        Cmd2 = (byte)PresetSelectControl.OutDataPreset;
    }

    /// <summary>
    /// </summary>
    /// <param name="tc"></param>
    public OutPreset(TimeCode tc)
    {
        var data = tc.ToBinaryCodedDecimal();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.PresetSelectControl, data.Length);
        Cmd2 = (byte)PresetSelectControl.OutDataPreset;
        Data = data;
    }
}
