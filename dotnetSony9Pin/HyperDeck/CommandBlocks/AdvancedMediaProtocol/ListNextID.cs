using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

internal class ListNextID : CommandBlock
{
    /// <summary>
    /// when x = 0 single clip request when x = 1,
    /// #clips can be specified in the send data
    /// </summary>
    public ListNextID(byte numberOfClips)
    {
        if (numberOfClips > 0)
        {
            var data = new byte[1];
            data[0] = numberOfClips;

            Cmd1DataCount = ToCmd1DataCount(CommandFunction.xxxRequest, data.Length);
            Cmd2 = (byte)AdvancedMediaProtocol.ListNextID;
            Data = data;
        }
        else
        {
            Cmd1 = CommandFunction.xxxRequest;
            Cmd2 = (byte)AdvancedMediaProtocol.ListNextID;
        }
    }
}
