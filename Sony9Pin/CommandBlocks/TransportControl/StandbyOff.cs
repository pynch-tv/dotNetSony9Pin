namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class StandbyOff : CommandBlock
{
    /// <summary>
    ///     Turns off Standby mode. For VTR, this causes the machine to unthread in Stop. Affects EE/Tape selection. Available
    ///     only in Stop mode.
    /// </summary>
    public StandbyOff()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.StandbyOff;
    }
}
