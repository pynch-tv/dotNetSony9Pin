using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class GetEvent : CommandBlock
{
    /// <summary>
    /// Each Odetics communication port maintains a queue of events.
    /// The return of this function depends of the event that has occurred into the system.
    /// </summary>
    public GetEvent()
    {
        Cmd1 = CommandFunction.evsRequest;
        Cmd2 = (byte)EVSAdditionalCommands.GetEvent;
    }
}
