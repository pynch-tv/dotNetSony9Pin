using System.Text;
using lathoub.dotNetSony9Pin.Extenions;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class NetCopyClipIdLsm : CommandBlock
{
    /// <summary>
    /// Copy a clip to target machine.
    /// The source and target clips are identified by a Lsm id.
    /// The target machine is identified by the target Lsm id
    /// 
    /// Format : B9 0D ‘X’ + ID LSM (8 bytes), where ‘X’ is the function selector :
    ///    X = ‘S’ (53) => set the source clip ID
    ///    X = ‘T’ (54) => set the target clip ID and do the copy
    /// </summary>
    public NetCopyClipIdLsm(string lsmId, byte direction)
    {
        if (direction != 'S' && direction != 'T')
            throw new ArgumentOutOfRangeException(nameof(lsmId), "direction must start with 'S' or 'T'");

        var dataClipId = Encoding.ASCII.GetBytes(lsmId.FixedLength(8));

        var data = new byte[] { direction }.Concat(dataClipId).ToArray();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.NetMoveClipIdLsm;
        Data = data;
    }
}
