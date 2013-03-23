using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.Events;
using MoreLinq;

namespace MixPlanner.DomainModel
{
    public interface IMix : IEnumerable<IMixItem>
    {
        IEnumerable<Track> Tracks { get; }
        void Add(Track track);
        void Insert(Track track, int insertIndex);
        void Insert(IEnumerable<Track> tracks, int insertIndex);
        void Remove(IMixItem item);
        void Reorder(IMixItem item, int newIndex);
        void AdjustPlaybackSpeed(IMixItem item, double value);
        void RemoveRange(IEnumerable<IMixItem> items);
        bool Contains(Track track);
        void ResetPlaybackSpeed(IMixItem item);
        bool IsEmpty { get; }
        IMixItem this[int index] { get; }
        int Count { get; }
        double CalculateAverageActualBpm();
        double CalculateAverageOriginalBpm();
        void AutoAdjustBpms();
        IMixItem GetMixItem(Track track);
        IEnumerable<IMixItem> GetUnknownTracks();
        int IndexOf(IMixItem item);
        IDisposable DisableRecalcTransitions();
        void MoveToEnd(IMixItem track);
        IMixItem GetPreceedingItem(IMixItem item);
        IMixItem GetFollowingItem(IMixItem item);
    }

    public class Mix : IMix
    {
        readonly IDispatcherMessenger messenger;
        readonly IActualTransitionDetector transitions;
        readonly ILimitingPlaybackSpeedAdjuster playbackSpeedAdjuster;
        readonly IList<MixItem> items;
        bool isRecalcTransitionsEnabled = true;

        public Mix(
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
            items = new List<MixItem>();
            messenger.Register<ConfigSavedEvent>(this, _ => RecalcTransitions());
            messenger.Register<TrackUpdatedEvent>(this, OnTrackUpdated);
        }

        void OnTrackUpdated(TrackUpdatedEvent obj)
        {
            items.Where(i => i.Track.Equals(obj.Track))
                .ForEach(ResetPlaybackSpeed);
        }

        public IEnumerable<Track> Tracks { get { return items.Select(i => i.Track); } }

        public IMixItem this[int index]
        {
            get { return items[index]; }
        }

        public bool IsEmpty { get { return !items.Any(); } }

        public int Count
        {
            get { return items.Count; }
        }

        public double CalculateAverageActualBpm()
        {
            return items
                .Select(i => i.PlaybackSpeed.ActualBpm)
                .Where(d => !double.IsNaN(d))
                .Average();
        }

        public double CalculateAverageOriginalBpm()
        {
            return items
                .Select(i => i.Track.OriginalBpm)
                .Where(d => !double.IsNaN(d))
                .Average();
        }

        public void AutoAdjustBpms()
        {
            double targetBpm = CalculateAverageOriginalBpm();

            foreach (MixItem item in items)
            {
                double increase = 1 + playbackSpeedAdjuster
                    .GetSuggestedIncrease(item.GetDefaultPlaybackSpeed(), targetBpm);

                AdjustPlaybackSpeed(item, increase);
            }
        }

        public IMixItem GetMixItem(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return items.FirstOrDefault(i => i.Track.Equals(track));
        }

        public IEnumerable<IMixItem> GetUnknownTracks()
        {
            return items.Where(i => i.IsUnknownKeyOrBpm);
        }

        public int IndexOf(IMixItem item)
        {
            return items.IndexOf((MixItem)item);
        }

        public IDisposable DisableRecalcTransitions()
        {
            isRecalcTransitionsEnabled = false;
            return new ActionOnDispose(EnableRecalcTransitions);
        }

        public void MoveToEnd(IMixItem track)
        {
            if (track == null) throw new ArgumentNullException("track");
            
            Reorder(track, Count);
        }

        public IMixItem GetPreceedingItem(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            int preceedingIndex = IndexOf(item) - 1;
            return preceedingIndex < 0 ? null : this[preceedingIndex];
        }

        public IMixItem GetFollowingItem(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            int followingIndex = IndexOf(item) + 1;
            return followingIndex == Count ? null : this[followingIndex];
        }

        void EnableRecalcTransitions()
        {
            isRecalcTransitionsEnabled = true;
            RecalcTransitions();
        }

        public void Remove(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            items.Remove((MixItem)item);
            messenger.SendToUI(new TrackRemovedFromMixEvent(item));
            RecalcTransitions();
        }

        public void Reorder(IMixItem item, int newIndex)
        {
            if (item == null) throw new ArgumentNullException("item");

            var mixItem = (MixItem) item;

            var oldIndex = items.IndexOf(mixItem);

            // Adjust newIndex to account for the fact we are removing an item
            // before inserting.
            if (oldIndex < newIndex)
                newIndex = Math.Max(newIndex - 1, 0);
            else
                newIndex = Math.Min(newIndex, items.Count);

            items.Remove(mixItem);
            items.Insert(newIndex, mixItem);
            messenger.SendToUI(new TrackRemovedFromMixEvent(item));
            messenger.SendToUI(new TrackAddedToMixEvent(item, newIndex));
            RecalcTransitions();
        }

        public void AdjustPlaybackSpeed(IMixItem item, double value)
        {
            if (item == null) throw new ArgumentNullException("item");
            ((MixItem)item).SetPlaybackSpeed(value);
            messenger.SendToUI(new PlaybackSpeedAdjustedEvent(item));
            RecalcTransitions();
        }

        public void RemoveRange(IEnumerable<IMixItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            items.ForEach(Remove);
        }

        public bool Contains(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return items.Any(m => m.Track.Equals(track));
        }

        public void Add(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            Insert(track, items.Count);
        }

        public void Insert(Track track, int insertIndex)
        {
            if (track == null) throw new ArgumentNullException("track");

            var item = CreateItem(track, insertIndex);
            
            items.Insert(insertIndex, item);

            messenger.SendToUI(new TrackAddedToMixEvent(item, insertIndex));
            RecalcTransitions();
        }

        public void Insert(IEnumerable<Track> tracks, int insertIndex)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");

            foreach (var track in tracks)
                Insert(track, insertIndex++);
        }

        public void RecalcTransitions()
        {
            if (!isRecalcTransitionsEnabled)
                return;

            if (!items.Any())
                return;

            var intro = items[0];
            if (intro.Transition.Strategy != null)
            {
                // First track should not have any strategy - should just be intro. 
                // Need to clean this up a bit.
                intro.Transition = transitions.GetTransitionBetween(null, intro.PlaybackSpeed);
                messenger.SendToUI(new TransitionChangedEvent(intro));
            }

            for (var i = 0; i + 1 < items.Count; i++)
            {
                var previousItem = items[i];
                var item = items[i + 1];

                var oldTransition = item.Transition;
                var newTransition = transitions.GetTransitionBetween(
                    previousItem.PlaybackSpeed, item.PlaybackSpeed);

                if (newTransition.Strategy == oldTransition.Strategy)
                    continue;

                item.Transition = newTransition;

                messenger.SendToUI(new TransitionChangedEvent(item));
            }
        }

        MixItem CreateItem(Track track, int insertIndex)
        {
            PlaybackSpeed previous = GetPlaybackSpeedAtPosition(insertIndex - 1);

            PlaybackSpeed next = track.GetDefaultPlaybackSpeed();
            if (previous != null)
                next = playbackSpeedAdjuster.AutoAdjust(previous, next);

            Transition transition = transitions.GetTransitionBetween(previous, next);

            return new MixItem(this, track, transition, next);
        }

        PlaybackSpeed GetPlaybackSpeedAtPosition(int index)
        {
            if (items.Count == 0 || index >= items.Count || index < 0)
                return null;

            return items[index].PlaybackSpeed;
        }

        public void ResetPlaybackSpeed(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            var mixItem = (MixItem) item;
            mixItem.ResetPlaybackSpeed();
            messenger.SendToUI(new PlaybackSpeedAdjustedEvent(mixItem));
            RecalcTransitions();
        }

        public IEnumerator<IMixItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}