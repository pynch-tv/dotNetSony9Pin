namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class AntiClogTimerEnable : CommandBlock
{
    /// <summary>
    ///     Enables anti-clog timer.
    /// </summary>
    public AntiClogTimerEnable()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.AntiClogTimerEnable;
    }
}