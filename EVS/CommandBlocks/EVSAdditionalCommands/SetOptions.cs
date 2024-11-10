using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class SetOptions : CommandBlock
{
    /// <summary>
    /// Format : B4.10 + options (4 bytes) 
    /// 
    /// For now, only bit 0 is used :
    /// If it is set, it is allowed to use guard bands, i.e.outside the range short in <- -> short out 
    /// </summary>
    public SetOptions(byte options)
    {
        var data = new byte[] { options };

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.SetOptions;
        Data = data;
    }
}
