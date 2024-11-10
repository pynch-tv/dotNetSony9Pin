using lathoub.dotNetSony9Pin;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace Pynch.Sony9Pin.Core.Odetics.CommandBlocks.xxxRequest;

public class EraseSegment : CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startTC"></param>
    /// <param name="endTC"></param>
    public EraseSegment(TimeCode startTC, TimeCode endTC)
    {
        Cmd1 = CommandFunction.xxxRequest;
        Cmd2 = (byte)xxxRequest.EraseSegment;
    }
}