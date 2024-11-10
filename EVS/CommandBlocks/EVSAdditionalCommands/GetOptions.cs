using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetOptions : CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    public GetOptions()
    {
        Cmd1 = CommandFunction.evsRequest;
        Cmd2 = (byte)EVSAdditionalCommands.GetOptions;
    }
}
