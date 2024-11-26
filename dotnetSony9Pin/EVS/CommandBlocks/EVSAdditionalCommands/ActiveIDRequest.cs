using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public class ActiveIDRequest : CommandBlock
{
    /// <summary>
    /// Returns the clip ID currently loaded on the channel. This function can be useful if the clip has
    /// been loaded by an external source(another protocol that also has the control on this channel
    /// for instance). 
    /// </summary>
    public ActiveIDRequest()
    {
        var data = new byte[1];

        data[0] = 0x01;

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.evsRequest, data.Length);
        Cmd2 = (byte)EVSAdditionalCommands.GeneralInformation;
        Data = data;
    }
}
