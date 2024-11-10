namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.PresetSelectControl;

/// <summary>
/// 
/// </summary>
public class OutFlagReset : CommandBlock
{
    /// <summary>
    ///     Sets the Video out point to the value displayed on the _slave. This is the value of the selected tape timer.
    /// </summary>
    public OutFlagReset()
    {
        Cmd1 = CommandFunction.PresetSelectControl;
        Cmd2 = (byte)PresetSelectControl.OutFlagReset;
    }
}
