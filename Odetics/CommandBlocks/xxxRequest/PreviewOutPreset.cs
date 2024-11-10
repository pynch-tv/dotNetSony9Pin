using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace Pynch.Sony9Pin.Core.Odetics.CommandBlocks.xxxRequest;

public class PreviewOutPreset : CommandBlock
{
    public PreviewOutPreset()
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.PreviewOutPreset;
    }
}