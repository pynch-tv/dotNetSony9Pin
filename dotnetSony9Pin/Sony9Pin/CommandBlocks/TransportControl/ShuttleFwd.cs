﻿namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class ShuttleFwd : CommandBlock
{
    /// <summary>
    ///     When these commands are received the _slave device will move forward with the speed indicated by DATA-1 and DATA-2.
    /// </summary>
    public ShuttleFwd(byte data1)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 1;
        Cmd2 = (byte)TransportControl.ShuttleFwd;
        Data = [data1];
    }

    /// <summary>
    ///     When these commands are received the _slave device will move forward with the speed indicated by DATA-1 and DATA-2.
    /// </summary>
    public ShuttleFwd(byte data1, byte data2)
    {
        Cmd1 = CommandFunction.TransportControl;
        DataCount = 2;
        Cmd2 = (byte)TransportControl.ShuttleFwd;
        Data = [data1, data2];
    }
}
