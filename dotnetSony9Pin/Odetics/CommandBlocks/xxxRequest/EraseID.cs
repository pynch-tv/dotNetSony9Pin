using System.Diagnostics;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.xxxRequest;

public class EraseID : CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    public EraseID()
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.EraseID;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public EraseID(byte[] id)
    {
        Debug.Assert(id != null);
        if (id.Length != 8)
            throw new ArgumentException("id needs to be exactly 8 bytes long.");

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.xxxRequest, id.Length);
        Cmd2 = (byte)xxxRequest.EraseID;
        Data = id;
    }
}