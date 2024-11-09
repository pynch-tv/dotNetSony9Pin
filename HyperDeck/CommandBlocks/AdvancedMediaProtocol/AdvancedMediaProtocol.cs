namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.AdvancedMediaProtocol;

/// <summary>
///     The transport control.
/// </summary>
public enum AdvancedMediaProtocol : byte
{
    /// <summary>
    ///     The stop.
    /// </summary>
    AutoSkip = 0x01,

    /// <summary>
    ///     The stop.
    /// </summary>
    ListNextID = 0x15,

    /// <summary>
    ///     The stop.
    /// </summary>
    ClearPlaylist = 0x29,

    /// <summary>
    ///     The stop.
    /// </summary>
    SetPlaybackLoop = 0x42,

    /// <summary>
    ///     The stop.
    /// </summary>
    SetStopMode = 0x44,

    /// <summary>
    ///     The stop.
    /// </summary>
    AppendPreset = 0x16,
}
