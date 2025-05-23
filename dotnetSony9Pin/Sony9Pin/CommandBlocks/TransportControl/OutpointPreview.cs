﻿namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.TransportControl;

/// <summary>
/// 
/// </summary>
public class OutpointPreview : CommandBlock
{
    /// <summary>
    ///     Sends transport to preset out point if Insert mode is preset.
    /// </summary>
    public OutpointPreview()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.OutpointPreview;
    }
}
