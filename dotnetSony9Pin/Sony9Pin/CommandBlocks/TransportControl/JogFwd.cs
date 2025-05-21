using System;

namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class JogFwd : CommandBlock
{
    /// <summary>
    /// When only DATA-1 is given, the speed wil be given by
    /// where N is the value of DATA-1. Some sample values are:
    /// 
    ///  Speed   Speed data
    ///   0.1x     32 (20H)
    ///   1.0x     64 (40H)
    ///   2.9x     79 (4FH)
    ///  48.7x    118 (76H)
    ///
    /// Tape Speed = 10 ^ ((N / 32) - 2) x play speed.
    ///
    /// </summary>
    public JogFwd(byte data1)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 1;
        Cmd2 = (byte)TransportControl.JogFwd;
        Data = [data1];
    }

    /// <summary>
    /// When these commands are received the slave device will move forward with the speed 
    /// indicated by DATA-1 and DATA-2.
    ///
    /// When a more precise speed value is required, then DATA-2 will be added. The speed
    /// formula for this case is
    ///
    /// Tape Speed = 10^((N/32)-2) + N'/256*(10^(((N+1)/32)-2)-10^((N/32)-2))
    /// 
    /// where N is the value of DATA-1 and N' is the value of DATA-2.
    /// 
    /// auth - in more standard terms, the formula says that DATA-2 is used to linealy 
    /// interpolate between the value given by N and that of N+1.
    ///
    /// The maximum jog speed is set in the System:System menu. The maximum Var speed is 
    /// 3X play speed. The maximum shuttle speed is 50X play speed.
    ///
    /// auth - There is considerable controversy over the minimum speed. For a speed value 
    /// of 0, the above formula with only DATA-1 gives 10^-2, or .01 x play speed. The standard 
    /// states that when a speed between 0 and the minumum is given, the slave moves at minimum 
    /// speed. In fact, many editors and control systems intend a "Shuttle 0" command ( 21 13 00 ) 
    /// to pause the device and have it stop without disengaging. Devices which fail to do so will 
    /// creep about 1 frame/second in this situation.
    ///
    /// </summary>
    public JogFwd(byte data1, byte data2)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 2;
        Cmd2 = (byte)TransportControl.JogFwd;
        Data = [data1, data2];
    }
}
