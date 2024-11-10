namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class VarFwd : CommandBlock
{
    /// <summary>
    ///     When these commands are received the _slave device will move forward with the speed indicated by DATA-1 and DATA-2.
    /// </summary>
    public VarFwd()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.VarFwd;
    }
}
