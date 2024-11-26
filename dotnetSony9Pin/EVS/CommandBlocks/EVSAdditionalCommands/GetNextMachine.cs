using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetNextMachine : CommandBlock
{
    public GetNextMachine()
    {
        Cmd1 = CommandFunction.evsRequest;
        Cmd2 = (byte)EVSAdditionalCommands.GetNextMachine;
    }

}
