using System.Text;
using dotNetSony9Pin.Extenions;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class NetMoveClipIdVDCP : CommandBlock
{
    /// <summary>
    /// Move a clip to target machine.
    /// The source clip is identified by a VDCP id.
    /// </summary>
    /// <param name="clipId"></param>
    /// <param name="machineTargetId"></param>
    public NetMoveClipIdVDCP(string clipId, byte machineTargetId)
    {
        var dataClipId = Encoding.ASCII.GetBytes(clipId.FixedLength(8));

        var data = dataClipId.Concat([machineTargetId]).ToArray();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.NetMoveClipIdVDCP;
        Data = data;
    }
}
