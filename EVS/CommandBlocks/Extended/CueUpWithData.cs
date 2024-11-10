using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.Extended;

public class CueUpWithData : CommandBlock
{
    /// <summary>
    /// If the current clip is a train then goto Live on this train.
    /// If the current clip is not a train then cueup on the clip IN point.
    /// </summary>
    public CueUpWithData()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.CueUpWithData;
    }

    /// <summary>
    /// If the given clip is a train, then load at out – 2 (also works on a remote train).
    /// If the given clip is not a train then load and cueup on the clip point IN.
    /// If ID is a train that has never been started (IN point equal to OUT point) than does nothing.
    /// </summary>
    public CueUpWithData(int a, int b)
    {
        var data = new byte[8];

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, data.Length);
        Cmd2 = (byte)TransportControl.CueUpWithData;
        Data = data;
    }

    /// <summary>
    /// Load the given clip ID and cueUp on the given time code. If the given time code doesn’t exist
    /// and ID is a train then cueup on the train out point(it is useful for controller using only
    /// TC=00:00:00:00 like the TDC100).
    /// If ID is a clip and TC = 00:00:00:00 then cueUp on the clip IN point.
    /// If ID is a clip and TC doesn’t exist in the clip then does nothing.
    /// </summary>
    public CueUpWithData(int a, int b, int c)
    {
        var data = new byte[0x0C];

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, data.Length);
        Cmd2 = (byte)TransportControl.CueUpWithData;
        Data = data;
    }

}
