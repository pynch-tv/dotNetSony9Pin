using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.xxxRequest;

public class ListFirstID : CommandBlock
{
    public ListFirstID()
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.ListFirstID;
    }
}