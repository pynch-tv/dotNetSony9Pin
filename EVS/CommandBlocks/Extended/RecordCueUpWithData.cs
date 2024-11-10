using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

namespace lathoub.dotNetSony9Pin.EVS.CommandBlocks.Extended;

public class RecordCueUpWithData : CommandBlock
{
    /// <summary>
    /// RecordCueUpWithData with no ID and no time code. Since the EVS server doesn’t support
    /// clip editing, a new clip ID is created with an automatic ID.For IDLouth, the new clip ID will
    /// have the format CXXXXXXX where XXXXXXX is a number generated inside the server.
    /// The time code corresponding to the IN point of the newly created clip will be 00:00:00:00. 
    /// </summary>
    public RecordCueUpWithData()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.RecordCueUpWithData;
    }

    /// <summary>
    /// This function is identical to the function A0.02 excepted for the time code corresponding to
    /// the IN point of the newly created clip will be the one given in the command.
    /// </summary>
    /// <param name="tc"></param>
    public RecordCueUpWithData(TimeCode tc)
    {
        var data = tc.ToBinaryCodedDecimal();

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, data.Length);
        Cmd2 = (byte)TransportControl.RecordCueUpWithData;
        Data = data;
    }

    /// <summary>
    /// A8.02 and AC.02 
    /// 
    /// They have the same behaviour as explained in the Odetics protocol documentation excepted if
    /// the given ID already exists.In this case, the command is simply refused.
    /// This function reset the out preset value. The function uses the couple Preroll/CueUp (see
    /// CueUpWithData) to indicate the command status. If, after the command, these two bits are
    /// not set then an error occurred. 
    /// </summary>

}
