namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class AutoModeOn : CommandBlock
{
    /// <summary>
    /// </summary>
    public AutoModeOn()
    {
        Cmd1 = Cmd1.TransportControl;
        Cmd2 = (byte)PresetSelectControl.PresetSelectControl.AutoModeOn;
    }
}