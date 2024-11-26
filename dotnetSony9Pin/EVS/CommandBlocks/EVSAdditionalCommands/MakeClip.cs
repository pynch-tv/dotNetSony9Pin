using System.Text;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class MakeClip : CommandBlock
{
    /// <summary>
    /// There is two groups of functions to create a clip.
    /// First group is related to ID LSM:
    ///    - B4.04: only use a format like 111A
    ///    - B6.04: like the previous function but with a guardband.
    ///    - B7.04: use a full LSM ID like 113B/00 for the local machine or 114C/23
    ///             for a networked machine.
    ///    - B9.04: use a full LSM ID and a guardband. 
    ///
    /// Second group is related to ID Louth:
    ///    - B8.04 only use a clip ID Louth.
    ///    - BA.04 use an ID Louth and a guardband.
    ///    
    /// The guardband is always transmitted on two bytes. 
    /// </summary>
    /// <param name="id"></param>
    public MakeClip(string id)
    {
        var data = Encoding.ASCII.GetBytes(id[8..].TrimEnd());

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.MakeClip;
        Data = data;
    }
}
