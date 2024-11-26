using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.xxxRequest;

public class PreviewOutPreset : CommandBlock
{
    public PreviewOutPreset()
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.PreviewOutPreset;
    }
}