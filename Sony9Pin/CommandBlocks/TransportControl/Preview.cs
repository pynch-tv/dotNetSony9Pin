namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class Preview : CommandBlock
{
    /// <summary>
    ///     When one of these commands is received the _slave goes into the indicated mode
    /// </summary>
    public Preview()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.Preview;
    }
}
