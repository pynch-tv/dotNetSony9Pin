﻿using dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace dotNetSony9Pin.Odetics.CommandBlocks.TapeControl;

/// <summary>
/// Note: This command has been added to address video disk recorder specific requirements
/// 
/// This command immediately aborts auto play of the current video segment specified
///  by the current auto mode preset, and performs the processing associated with the 
/// end of the current auto mode preset (see 40.41, Auto Mode On).
/// </summary>
public class DeleteIdFromArchive : CommandBlock
{
    public DeleteIdFromArchive()
    {
        Cmd1 = CommandFunction.TapeControl;
        Cmd2 = (byte)TapeControl.DeleteIdFromArchive;
    }
}
