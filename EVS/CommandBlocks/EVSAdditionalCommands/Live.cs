using System.Text;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class Live : CommandBlock
{
    /// <summary>
    /// This command goes to live on the current channel with the camera specified as argument (8
    /// bytes for the camera id). The id can be specified as an ID LSM or an ID Louth.It can also be
    /// a remote camera(i.e.not local). 
    /// </summary>
    public Live(string id)
    {
        var data = Encoding.ASCII.GetBytes(id);

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.Live;
        Data = data;
    }
}
