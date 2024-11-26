using System.Text;
using dotNetSony9Pin.Extenions;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class NetCopyClipIdVDCP : CommandBlock
{
    /// <summary>
    /// Copy a clip to target machine.
    /// The source and target clips are identified by a VDCP id.
    /// This function is split into two parts :
    /// Format :
    ///    B8 0C + source VDCP ID(8 bytes)
    ///    First, set the source clip ID :
    ///    B9 0C + ID target machine(1 byte) + target VDCP ID(8 bytes)
    ///    Then do the copy
    ///
    /// </summary>
    public NetCopyClipIdVDCP(string sourceClipId)
    {
        var data = Encoding.ASCII.GetBytes(sourceClipId.FixedLength(8));

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.NetCopyClipIdVDCP;
        Data = data;
    }

    public NetCopyClipIdVDCP(byte machineTargetId, string taregtClipId)
    {
        var dataClipId = Encoding.ASCII.GetBytes(taregtClipId.FixedLength(8));

        var data = dataClipId.Concat([machineTargetId]).ToArray();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.NetCopyClipIdVDCP;
        Data = data;
    }
}
