namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.Return;

/// <summary>
///     The device type event args.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceTypeEventArgs"/> class.
/// </remarks>
/// <param name="DeviceDescription">
/// The device name.
/// </param>
public class DeviceTypeEventArgs(DeviceDescription? dd) : EventArgs
{
    /// <summary>
    ///     The device name.
    /// </summary>
    public readonly DeviceDescription DeviceDescription = dd;
}