using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IIntelligentInserter
    {
        InsertResults GetBestInsertIndex(Mix mix, Track track);
    }

    public class IntelligentInserter : IIntelligentInserter
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public IntelligentInserter(IMixingStrategiesFactory strategiesFactory)
        {
            if (strategiesFactory == null) throw new ArgumentNullException("strategiesFactory");
            strategies = strategiesFactory.GetStrategiesInPreferredOrder();
        }

        public InsertResults GetBestInsertIndex(Mix mix, Track track)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (track == null) throw new ArgumentNullException("track");

            if (mix.IsEmpty)
                return InsertResults.SuccessEmptyMix();

            var speed = track.GetDefaultPlaybackSpeed();

            for (int i = mix.Count; i >= 0; i--)
            {
                var trackBefore = i == 0 ? null : mix[i - 1].PlaybackSpeed;
                IMixingStrategy startStrategy = null;
                if (trackBefore != null)
                {
                    startStrategy = strategies.FirstOrDefault(s => s.IsCompatible(trackBefore, speed));
                    if (startStrategy == null)
                        continue;
                }
                
                var trackAfter = i == mix.Count ? null : mix[i].PlaybackSpeed;
                IMixingStrategy endStrategy = null;
                if (trackAfter != null)
                {
                    endStrategy = strategies.FirstOrDefault(s => s.IsCompatible(speed, trackAfter));
                    if (endStrategy == null)
                        continue;
                }

                return InsertResults.Success(startStrategy, i, endStrategy);
            }
            return InsertResults.Failure();

        }
    }
}