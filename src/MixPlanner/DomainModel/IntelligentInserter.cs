using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IIntelligentInserter
    {
        InsertResults GetBestInsertIndex(IMix mix, Track track);
    }

    public class IntelligentInserter : IIntelligentInserter
    {
        readonly IPlaybackSpeedAdjuster adjuster;
        readonly IEnumerable<IMixingStrategy> strategies;

        public IntelligentInserter(
            IMixingStrategiesFactory strategiesFactory,
            IPlaybackSpeedAdjuster adjuster)
        {
            this.adjuster = adjuster;
            if (strategiesFactory == null) throw new ArgumentNullException("strategiesFactory");
            if (adjuster == null) throw new ArgumentNullException("adjuster");
            strategies = strategiesFactory.GetStrategiesInPreferredOrder();
        }

        public InsertResults GetBestInsertIndex(IMix mix, Track track)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (track == null) throw new ArgumentNullException("track");

            if (mix.IsEmpty)
                return InsertResults.SuccessEmptyMix();

            var speed = track.GetDefaultPlaybackSpeed();

            InsertResults insertResults;
            if (TryInsert(mix, speed, out insertResults))
                return insertResults;

            // Brute force algo - if we couldn't shoehorn the track in anywhere
            // at it's default playback speed, try again at -6%, -3%, +3%, +6%

            IEnumerable<double> adjustmentsToTry = GetAdjustmentsToTry();
            foreach (double adjustment in adjustmentsToTry)
            {
                var adjustedSpeed = speed.AsIncreasedBy(adjustment);
                if (TryInsert(mix, adjustedSpeed, out insertResults))
                    return insertResults;
            }

            return InsertResults.Failure();
        }

        static IEnumerable<double> GetAdjustmentsToTry()
        {
            var adjustmentsToTry = new List<double>();
            for (int i = -PitchFaderStep.NumberOfSteps; i <= PitchFaderStep.NumberOfSteps; i++)
            {
                if (i == 0) // already tested unadjusted (0)
                    continue;

                adjustmentsToTry.Add(i*PitchFaderStep.Value);
            }
            
            // Start from the smallest adjustments, and work our way out
            return adjustmentsToTry.OrderBy(Math.Abs);
        }

        bool TryInsert(IMix mix, PlaybackSpeed speed, out InsertResults insertResults)
        {
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
                insertResults = InsertResults.Success(startStrategy, i, endStrategy, speed.Adjustment);
                return true;
            }

            insertResults = null;
            return false;
        }
    }
}