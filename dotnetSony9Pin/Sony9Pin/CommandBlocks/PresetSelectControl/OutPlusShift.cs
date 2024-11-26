namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.PresetSelectControl;

/// <summary>
/// 
/// </summary>
public class OutPlusShift : CommandBlock
{
    /// <summary>
    ///     Increments the Video out point by one frame.
    /// </summary>
    public OutPlusShift()
    {
        Cmd1 = CommandFunction.PresetSelectControl;
        Cmd2 = (byte)PresetSelectControl.OutPlusShift;
    }
}
