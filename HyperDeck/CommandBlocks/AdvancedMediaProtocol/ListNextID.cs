using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class ListNextID : CommandBlock
{
    /// <summary>
    /// when x = 0 single clip request when x = 1,
    /// #clips can be specified in the send data
    /// </summary>
    public ListNextID(int id)
    {
        var data = new byte[5];

        Cmd1DataCount = ToCmd1DataCount((Cmd1)0x8, data.Length);
        Cmd2 = (byte)AdvancedMediaProtocol.ListNextID;
        Data = data;
    }
}
