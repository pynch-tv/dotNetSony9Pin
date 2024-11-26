using System.Text;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class SetInOut : CommandBlock
{
    /// <summary>
    /// Update clip short in & short out
    /// First use the functions 40.10 and 40.11 to define the time code Short IN and time code Short
    /// OUT of the clip to be updated, identified by the given clip id.
    /// 
    /// Format :
    /// B4.12 + ID LSM format like 111A for the local machine(4 bytes)
    /// B7.12 + ID LSM format like 113B/00 for the local machine or 114C/23 for a
    /// networked machine(7 bytes)
    /// B8.12 + ID Louth(8 bytes)
    /// </summary>
    /// <param name="inOut"></param>
    public SetInOut(string id)
    {
        var len = id.Length;
        if (len != 4 && len != 7 && len != 8)
            throw new ArgumentOutOfRangeException(nameof(id), "id must be 4, 7 or 8 characters long");

        var data = Encoding.ASCII.GetBytes(id);

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.SetInOut;
        Data = data;
    }
}
