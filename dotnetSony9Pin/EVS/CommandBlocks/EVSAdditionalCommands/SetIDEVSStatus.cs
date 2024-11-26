using System.Text;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class SetIDEVSStatus : CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    public SetIDEVSStatus(string clipId, byte bitmap, byte status)
    {
        var data = Encoding.ASCII.GetBytes(clipId[8..].TrimEnd());
        Array.Resize(ref data, 10);

        data[9] = bitmap;
        data[10] = status;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.SetIDEVSStatus;
        Data = data;
    }
}
