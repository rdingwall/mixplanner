using System;
using System.Collections.Generic;

namespace MixPlanner.DomainModel
{
    public interface IMixFactory
    {
        IMix Create();
        IMix Create(IEnumerable<Tuple<Track, double>> tracks, string filename);
    }

    public class MixFactory : IMixFactory
    {
        readonly IDispatcherMessenger messenger;
        readonly IActualTransitionDetector transitions;
        readonly ILimitingPlaybackSpeedAdjuster playbackSpeedAdjuster;

        public MixFactory(
            IDispatcherMessenger messenger, 
            IActualTransitionDetector transitions, 
            ILimitingPlaybackSpeedAdjuster playbackSpeedAdjuster)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (transitions == null) throw new ArgumentNullException("transitions");
            if (playbackSpeedAdjuster == null) throw new ArgumentNullException("playbackSpeedAdjuster");
            this.messenger = messenger;
            this.transitions = transitions;
            this.playbackSpeedAdjuster = playbackSpeedAdjuster;
        }

        public IMix Create()
        {
            return new Mix(messenger, transitions, playbackSpeedAdjuster);
        }

        public IMix Create(IEnumerable<Tuple<Track, double>> tracks, string filename)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            if (filename == null) throw new ArgumentNullException("filename");
            return new Mix(messenger, transitions, playbackSpeedAdjuster, tracks, filename);
        }
    }
}