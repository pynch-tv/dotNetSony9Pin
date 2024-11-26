namespace dotNetSony9Pin.EVS.CommandBlocks.EVSAdditionalCommands;

public enum EVSAdditionalCommands : byte
{
    /// <summary>
    ///     The stop.
    /// </summary>
    GetEvent = 0x00,

    /// <summary>
    /// 
    /// </summary>
    SetIdForData = 0x02,

    /// <summary>
    /// 
    /// </summary>
    GetData = 0x03,

    /// <summary>
    /// 
    /// </summary>
    MakeClip = 0x04,

    /// <summary>
    /// 
    /// </summary>
    SetIDEVSStatus = 0x05,

    /// <summary>
    /// 
    /// </summary>
    GetKeyword = 0x07,

    /// <summary>
    /// 
    /// </summary>
    SetKeyword = 0x08,

    /// <summary>
    /// 
    /// </summary>
    GeneralInformation = 0x09,

    /// <summary>
    /// 
    /// </summary>
    NetMoveClipIdVDCP = 0x0A,

    /// <summary>
    /// 
    /// </summary>
    NetMoveClipIdLsm = 0x0B,

    /// <summary>
    /// 
    /// </summary>
    NetCopyClipIdVDCP = 0x0C,

    /// <summary>
    /// 
    /// </summary>
    NetCopyClipIdLsm = 0x0D,

    /// <summary>
    /// 
    /// </summary>
    GetFirstMachine = 0x0E,

    /// <summary>
    /// 
    /// </summary>
    GetNextMachine = 0x0F,

    /// <summary>
    /// 
    /// </summary>
    SetOptions = 0x10,

    /// <summary>
    /// 
    /// </summary>
    GetOptions = 0x11,

    /// <summary>
    /// 
    /// </summary>
    SetInOut = 0x12,

    /// <summary>
    /// 
    /// </summary>
    Live = 0x13,
}
