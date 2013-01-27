using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
using MoreLinq;

namespace MixPlanner.DomainModel
{
    public interface IMix
    {
        IEnumerable<Track> Tracks { get; }
        void Add(Track track);
        void Insert(Track track, int insertIndex);
        void Insert(IEnumerable<Track> tracks, int insertIndex);
        IEnumerable<MixItem> Items { get; }
        void Remove(MixItem item);
        void Reorder(MixItem item, int newIndex);
        void AdjustPlaybackSpeed(MixItem item, double value);
        void RemoveRange(IEnumerable<MixItem> items);
        bool Contains(Track track);
        void ResetPlaybackSpeed(MixItem item);
        bool IsEmpty { get; }
        MixItem this[int index] { get; }
        int Count { get; }
    }

    public class Mix : IMix
    {
        readonly IDispatcherMessenger messenger;
        readonly IActualTransitionDetector transitions;
        readonly IPlaybackSpeedAdjuster playbackSpeedAdjuster;
        readonly IList<MixItem> items;

        public Mix(
            IDispatcherMessenger messenger,
            IActualTransitionDetector transitions,
            IPlaybackSpeedAdjuster playbackSpeedAdjuster)
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

        public IEnumerable<Track> Tracks { get { return Items.Select(i => i.Track); } }

        public IEnumerable<MixItem> Items
        {
            get { return items; }
        }

        public MixItem this[int index]
        {
            get { return items[index]; }
        }

        public bool IsEmpty { get { return !items.Any(); } }

        public int Count
        {
            get { return items.Count; }
        }

        public void Remove(MixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            items.Remove(item);
            messenger.SendToUI(new TrackRemovedFromMixEvent(item));
            RecalcTransitions();
        }

        public void Reorder(MixItem item, int newIndex)
        {
            if (item == null) throw new ArgumentNullException("item");

            var oldIndex = items.IndexOf(item);

            // Adjust newIndex to account for the fact we are removing an item
            // before inserting.
            if (oldIndex < newIndex)
                newIndex = Math.Max(newIndex - 1, 0);
            else
                newIndex = Math.Min(newIndex, items.Count);

            items.Remove(item);
            items.Insert(newIndex, item);
            messenger.SendToUI(new TrackRemovedFromMixEvent(item));
            messenger.SendToUI(new TrackAddedToMixEvent(item, newIndex));
            RecalcTransitions();
        }

        public void AdjustPlaybackSpeed(MixItem item, double value)
        {
            if (item == null) throw new ArgumentNullException("item");
            item.SetPlaybackSpeed(value);
            messenger.SendToUI(new PlaybackSpeedAdjustedEvent(item));
            RecalcTransitions();
        }

        public void RemoveRange(IEnumerable<MixItem> items)
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

        public void ResetPlaybackSpeed(MixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            item.ResetPlaybackSpeed();
            messenger.SendToUI(new PlaybackSpeedAdjustedEvent(item));
            RecalcTransitions();
        }
    }
}