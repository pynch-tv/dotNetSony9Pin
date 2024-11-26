using System.Text;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class SetIdForData : CommandBlock
{
    /// <summary>
    /// A 12 characters data can be associated with a clip ID. This function selects the target ID that
    /// will be used with the command SetData.SetData contains the data that will be store with the
    /// previously defined clip ID.These functions have been split to keep the Sony protocol format
    /// for the commands. 
    /// </summary>
    /// <param name="id"></param>
    public SetIdForData(string id)
    {
        var data = Encoding.ASCII.GetBytes(id[12..].TrimEnd());

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.SetIdForData;
        Data = data;
    }
}
