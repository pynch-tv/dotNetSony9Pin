using System.Text;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetData : CommandBlock
{
    /// <summary>
    /// Return the data (.i.e the name as called in the XT Server) associated with the clip ID specified
    /// in the command.
    /// </summary>
    /// <param name="clipId"></param>
    public GetData(string clipId)
    {
        var data = Encoding.ASCII.GetBytes(clipId[8..].TrimEnd());

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.GetData;
        Data = data;
    }
}
