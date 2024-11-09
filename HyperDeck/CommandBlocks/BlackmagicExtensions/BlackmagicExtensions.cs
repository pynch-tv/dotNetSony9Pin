namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks.BlackmagicExtensions;

/// <summary>
///     The transport control.
/// </summary>
public enum BlackmagicExtensions : byte
{
    /// <summary>
    ///     The stop.
    /// </summary>
    BMDSeekToTimelinePosition = 0x02,

    /// <summary>
    ///     The stop.
    /// </summary>
    BMDSeekRelativeClip = 0x03,

    /// <summary>
    ///     The stop.
    /// </summary>
    BMDScrubTimelineDelta = 0x04,

    /// <summary>
    ///     The stop.
    /// </summary>
    BMDPlay = 0x05,

    /// <summary>
    ///     The stop.
    /// </summary>
    BMDClip = 0x06,
}
