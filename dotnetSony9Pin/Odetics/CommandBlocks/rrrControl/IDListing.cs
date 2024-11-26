using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.rrrControl;

/// <summary>
/// </summary>
public class IDListing : CommandBlock
{
    public IDListing()
    {
        Cmd1 = CommandFunction.rrrReturn;
        Cmd2 = (byte)rrrReturn.IDListing;
    }
}
