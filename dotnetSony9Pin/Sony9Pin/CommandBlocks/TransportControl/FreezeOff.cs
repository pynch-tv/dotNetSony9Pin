namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class FreezeOff : CommandBlock
{
    /// <summary>
    ///     This command un-freezes the output of the device.
    /// </summary>
    public FreezeOff()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.FreezeOff;
    }
}