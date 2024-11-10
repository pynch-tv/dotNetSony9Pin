namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class CueUpWithData : CommandBlock
{
    /// <summary>
    ///     Cues the _slave to the indicated time.
    /// </summary>
    public CueUpWithData(TimeCode tc)
    {
        var data = tc.ToBinaryCodedDecimal();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, data.Length);
        Cmd2 = (byte)TransportControl.CueUpWithData;
        Data = data;
    }
}
