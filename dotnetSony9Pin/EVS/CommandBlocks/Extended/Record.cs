namespace dotNetSony9Pin.EVS.CommandBlocks.Extended;

/// <summary>
/// This function is working differently if a RecordCueUpWithData has been sent or not. 
/// 
/// - if RecordCueUpWithData has been sent, then it is the record command as
///   defined for the Odetics protocol.
/// - If RecordCueUpWithData has not been sent before then there is two cases:
///   o If no clip has been previously preloaded or the current clip is not a
///     train then the default train is loaded on live.
///   o If the current clip is already a train then it goes on live.
///   
/// It is possible to play a clip while it is recording on another player channel.
/// </summary>
internal class Record
{
}
