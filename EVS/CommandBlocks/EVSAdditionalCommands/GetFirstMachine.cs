using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetFirstMachine : CommandBlock
{
    /// <summary>
    /// Get the list of machines in the SDTI network.
    /// Use this function to get the first machine in the SDTI network.
    /// </summary>
    public GetFirstMachine()
    {
        Cmd1 = CommandFunction.evsRequest;
        Cmd2 = (byte)EVSAdditionalCommands.GetFirstMachine;
    }
}
