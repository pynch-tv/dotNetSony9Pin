using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.TapeControl;

/// <summary>
/// </summary>
public class FormatTape : CommandBlock
{
    public FormatTape()
    {
        Cmd1 = CommandFunction.TapeControl;
        Cmd2 = (byte)TapeControl.FormatTape;
    }
}
