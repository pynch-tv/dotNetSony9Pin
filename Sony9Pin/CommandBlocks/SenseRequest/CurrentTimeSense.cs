namespace lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks.SenseRequest;

/// <summary>
/// 
/// </summary>
public class CurrentTimeSense : CommandBlock
{
    /// <summary>
    /// </summary>
    public CurrentTimeSense(TimeSenseRequest.TimeSenseRequest request)
    {
        var data = new[] { (byte)request };

        Cmd1 = CommandFunction.SenseRequest;
        DataCount = data.Length;
        Cmd2 = (byte)SenseRequest.CurrentTimeSense;
        Data = data;
    }
}
