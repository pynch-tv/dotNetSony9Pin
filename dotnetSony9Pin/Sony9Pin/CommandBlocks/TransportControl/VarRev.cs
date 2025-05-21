using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class VarRev : CommandBlock
{
    /// <summary>
    ///     When receiving one of the above commands, the _slave will start running in accordance with the speed Data defined
    ///     by DATA-1 and DATA-2. For the maximum and minimum speed see the 2X.12 Shuttle Fwd command.
    /// </summary>
    public VarRev(byte data1)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 1;
        Cmd2 = (byte)TransportControl.VarRev;
        Data = [data1];
    }

    public VarRev(byte data1, byte data2)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 2;
        Cmd2 = (byte)TransportControl.VarRev;
        Data = [data1, data2];
    }
}
