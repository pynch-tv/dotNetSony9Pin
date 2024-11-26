using System.Text;
using dotNetSony9Pin.Extenions;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class NetMoveClipIdLsm : CommandBlock
{
    /// <summary>
    /// Move a clip to target machine.
    /// The source clip is identified by a Lsm id.
    /// The target machine is identified by the target Lsm id
    /// 
    /// Format : B9 0B ‘X’ + LSM id (8 bytes), where ‘X’ is the function selector :
    ///      X = ‘S’ (53) => set the source clip id
    ///      X = ‘T’ (54) => set the target clip id and do the move
    ///      
    /// This function is split into two parts :
    /// First, set the source clip ID :
    ///     B9 0B 53 + Lsm source id(8 bytes)
    /// Then do the move
    ///     B9 0B 54 + Lsm target id(8 bytes)
    /// </summary>
    public NetMoveClipIdLsm(string lsmId, byte direction)
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
