using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class AutoSkip : CommandBlock
{
    public AutoSkip(sbyte numberOfClipsToSkip)
    {
        var data = new byte[1];

        data[0] = (byte)numberOfClipsToSkip;

        Cmd1DataCount = ToCmd1DataCount((Cmd1)0x8, data.Length);
        Cmd2 = (byte)AdvancedMediaProtocol.AutoSkip;
        Data = data;
    }
}
