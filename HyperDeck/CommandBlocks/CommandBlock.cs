using System.Text;
using lathoub.dotNetSony9Pin.Sony9Pin.CommandBlocks;

namespace lathoub.dotNetSony9Pin.HyperDeck.CommandBlocks;

public class CommandBlock : Sony9Pin.CommandBlocks.CommandBlock
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        switch (Cmd1)
        {
            case CommandFunction.PresetSelectControl:
                switch ((AdvancedMediaProtocol.AdvancedMediaProtocol)Cmd2)
                {
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.AppendPreset:
                        return ("AppendPreset");
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.SetPlaybackLoop:
                        return ("SetPlaybackLoop");
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.SetStopMode:
                        return ("SetStopMode");
                }
                break;

            case CommandFunction.xxxRequest:
                switch ((AdvancedMediaProtocol.AdvancedMediaProtocol)Cmd2)
                {
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.AutoSkip:
                        return ("AutoSkip");
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.ListNextID:
                        return ("ListNextID");
                }
                break;

            case CommandFunction.TransportControl:
                switch ((AdvancedMediaProtocol.AdvancedMediaProtocol)Cmd2)
                {
                    case AdvancedMediaProtocol.AdvancedMediaProtocol.ClearPlaylist:
                        return ("ClearPlaylist");
                }
                break;

            case CommandFunction.rrrReturn:
                switch ((BlackmagicExtensions.BlackmagicExtensions)Cmd2)
                {
                    case BlackmagicExtensions.BlackmagicExtensions.BMDSeekToTimelinePosition:
                        return ("BMDSeekToTimelinePosition");
                    case BlackmagicExtensions.BlackmagicExtensions.BMDSeekRelativeClip:
                        return ("BMDSeekRelativeClip");
                    case BlackmagicExtensions.BlackmagicExtensions.BMDScrubTimelineDelta:
                        return ("BMDScrubTimelineDelta");
                    case BlackmagicExtensions.BlackmagicExtensions.BMDPlay:
                        return ("BMDPlay");
                    case BlackmagicExtensions.BlackmagicExtensions.BMDClip:
                        return ("BMDClip");
                }
                break;
        }

        return base.ToString();
    }
}
