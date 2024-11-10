using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace Pynch.Sony9Pin.Core.Odetics.CommandBlocks.xxxRequest;

public class SetDeviceID : CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public SetDeviceID(byte[] id)
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.SetDeviceID;
    }
}