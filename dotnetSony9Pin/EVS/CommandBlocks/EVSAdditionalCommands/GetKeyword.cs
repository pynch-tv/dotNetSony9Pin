using System.Text;
using dotNetSony9Pin.Extenions;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetKeyword : CommandBlock
{
    /// <summary>
    /// Gets a keyword associated with a clip. A clip can have three keywords. A keyword is a 12
    /// characters long data.The format is: 
    /// Format : B9.07 + Clip ID (8 bytes) + keyword selector (1 byte)
    ///          The keyword selector is in the range[1..3] to select the target keyword.
    /// </summary>
    /// <param name="id"></param>
    public GetKeyword(string clipId, byte index)
    {
        if (index < 1 || index > 3)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be in the range [1..3]");

        var dataClipId = Encoding.ASCII.GetBytes(clipId.FixedLength(8));

        var data = dataClipId.Concat(dataClipId).Concat([index]).ToArray();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.GetKeyword;
        Data = data;
    }
}
