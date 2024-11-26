namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.PresetSelectControl;

/// <summary>
/// 
/// </summary>
public class InMinusShift : CommandBlock
{
    /// <summary>
    ///     Decrements the Video in point by one frame.
    /// </summary>
    public InMinusShift()
    {
        Cmd1 = CommandFunction.PresetSelectControl;
        Cmd2 = (byte)PresetSelectControl.InMinusShift;
    }
}
