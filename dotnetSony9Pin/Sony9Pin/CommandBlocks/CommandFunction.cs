﻿namespace dotNetSony9Pin.Sony9Pin.CommandBlocks;

/// <summary>
///     The command.
/// </summary>
public enum CommandFunction : byte
{
    /// <summary>
    ///     The system control.
    /// </summary>
    SystemControl = 0x0,

    /// <summary>
    ///     The return.
    /// </summary>
    Return = 0x1,

    /// <summary>
    ///     The transport control.
    /// </summary>
    TransportControl = 0x2,

    /// <summary>
    ///     The preset select control.
    /// </summary>
    PresetSelectControl = 0x4,

    /// <summary>
    ///     The sense request.
    /// </summary>
    SenseRequest = 0x6,

    /// <summary>
    ///     The sense return.
    /// </summary>
    SenseReturn = 0x7,

    /// <summary>
    ///     xxxRequest.
    /// </summary>
    rrrReturn = 0x8,

    /// <summary>
    ///     xxxRequest.
    /// </summary>
    xxxRequest = 0xA,

    /// <summary>
    ///     evsRequest.
    /// </summary>
    evsRequest = 0xB,

    /// <summary>
    ///     TapeControl.
    /// </summary>
    TapeControl = 0xC,

    /// <summary>
    ///     zzzReturn.
    /// </summary>
    zzzReturn = 0xD,
}