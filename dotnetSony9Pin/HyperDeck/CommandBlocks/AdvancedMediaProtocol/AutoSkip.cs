using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class AutoSkip : CommandBlock
{
    /// <summary>
    /// Number of clips to skip from current clip
    /// </summary>
    /// <param name="numberOfClipsToSkip"></param>
    public AutoSkip(sbyte numberOfClipsToSkip)
    {
        var data = new byte[1];

        data[0] = (byte)numberOfClipsToSkip;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.xxxRequest, data.Length);
        Cmd2 = (byte)AdvancedMediaProtocol.AutoSkip;
        Data = data;
    }
}
