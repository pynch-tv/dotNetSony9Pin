namespace lathoub.dotNetSony9Pin;

public class ConnectedEventArgs : EventArgs
{
    #region Fields

    /// <summary>
    ///     The index.
    /// </summary>
    public readonly bool Connected;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConnectedEventArgs" /> class.
    /// </summary>
    /// <param name="connected">
    ///     The memory.
    /// </param>
    public ConnectedEventArgs(bool connected)
    {
        Connected = connected;
    }

    #endregion
}
