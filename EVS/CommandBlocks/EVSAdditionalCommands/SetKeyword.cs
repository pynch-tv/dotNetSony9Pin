using System.Text;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;
using lathoub.dotNetSony9Pin.Extenions;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class SetKeyword : CommandBlock
{
    /// <summary>
    /// This function is split into two parts:
    /// Format :
    ///       B8.08 + clip ID
    ///          This function is used to store a clip ID for a future used for the keyword setting.
    ///       BD.08 + keyword(12 bytes) + keyword selector(1 byte)
    ///          This function is used to store keyword with the clip ID previously defined with the
    ///       B8.08 function.The keyword selector defines the target selector([range is [1..3]). 
    ///
    /// Events (see GetEvent here above) :
    /// 0x0C : clip id not found
    /// 
    /// The target clip ID is not reset after this function. It is then possible to use the BD.08 function
    /// three times with the appropriate keyword selector to define the three keywords associated
    /// with a clip.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public SetKeyword(string clipId, string keyword, byte index)
    {
        if (index < 1 || index > 3)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be in the range [1..3]");

        var dataClipId  = Encoding.ASCII.GetBytes(clipId.FixedLength(8));
        var dataKeyword = Encoding.ASCII.GetBytes(keyword.FixedLength(12));

        var data = dataClipId.Concat(dataKeyword).Concat([index]).ToArray();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.GetKeyword;
        Data = data;
    }
}
