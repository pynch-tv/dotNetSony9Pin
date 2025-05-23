﻿using System.Diagnostics;
using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.TransportControl;

[Obsolete("This command is no longer used. Use In Preset command in auto mode instead")]
public class CueUpWithData : CommandBlock
{
    /// <summary>
    ///     This command can be issued with no parameters. If zero bytes are specified,
    ///     the current time code position will be set to "00:00:00:00" and the currently
    ///     loaded Id will be used.
    /// </summary>
    public CueUpWithData()
    {
        Cmd1 = CommandFunction.TransportControl;
        Cmd2 = (byte)TransportControl.CueUpWithData;
    }

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

    /// <summary>
    ///     Cues the _slave to the indicated time.
    /// </summary>
    public CueUpWithData(byte[] id)
    {
        Debug.Assert(id != null);
        if (id.Length != 8)
            throw new ArgumentException("id needs to be exactly 8 bytes long.");

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, id.Length);
        Cmd2 = (byte)TransportControl.CueUpWithData;
        Data = id;
    }

    /// <summary>
    /// This command can be issued with a single Id parameter. If eight bytes of Data
    ///     are specified, the Id corresponding to the specified Id will be loaded, and the
    ///     current time code will be set to 00:00:00;00. If the specified does not currently
    ///     exist in the Video disk recorder, the cue command will not successfully complete.
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <param name="tc">
    /// </param>
    public CueUpWithData(byte[] id, TimeCode tc)
    {
        Debug.Assert(id != null);
        if (id.Length != 8)
            throw new ArgumentException("id needs to be exactly 8 bytes long.");

        var buffer = new byte[13];

        Buffer.BlockCopy(id, 0, buffer, 0, id.Length);
        Buffer.BlockCopy(tc.ToBinaryCodedDecimal(), 0, buffer, id.Length, 4);

        Cmd1DataCount = ToCmd1DataCount(CommandFunction.TransportControl, buffer.Length);
        Cmd2 = (byte)TransportControl.CueUpWithData;
        Data = buffer;
    }
}
