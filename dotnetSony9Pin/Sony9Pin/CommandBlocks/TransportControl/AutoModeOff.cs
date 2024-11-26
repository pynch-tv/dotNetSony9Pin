namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class AutoModeOff : CommandBlock
{
    /// <summary>
    /// </summary>
    public AutoModeOff()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)PresetSelectControl.PresetSelectControl.AutoModeOff;
    }
}