using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;

namespace MixPlanner.DomainModel
{
    public interface IMix
    {
        IEnumerable<Track> Tracks { get; }
        void Add(Track track);
        void Insert(Track track, int insertIndex);
        IEnumerable<MixItem> Items { get; }
        void Remove(MixItem item);
        void Reorder(MixItem item, int newIndex);
    }

    public class Mix : IMix
    {
        readonly IMessenger messenger;
        readonly ITransitionDetector transitions;
        readonly IList<MixItem> items;

        public Mix(
            IMessenger messenger,
            ITransitionDetector transitions)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            this.transitions = transitions;
            items = new List<MixItem>();
        }

        public IEnumerable<Track> Tracks { get { return Items.Select(i => i.Track); } }

        public IEnumerable<MixItem> Items
        {
            get { return items; }
        }

        public void Remove(MixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            items.Remove(item);
            messenger.Send(new TrackRemovedFromMixEvent(item));
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
            messenger.Send(new TrackRemovedFromMixEvent(item));
            messenger.Send(new TrackAddedToMixEvent(item, newIndex));
            RecalcTransitions();
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

            messenger.Send(new TrackAddedToMixEvent(item, insertIndex));
            RecalcTransitions();
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
                messenger.Send(new TransitionChangedEvent(intro));
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

                messenger.Send(new TransitionChangedEvent(item));
            }
        }

        MixItem CreateItem(Track track, int insertIndex)
        {
            var previousTrack = GetPlaybackSpeedAtPosition(insertIndex - 1);
            var playbackSpeed = track.GetDefaultPlaybackSpeed();
            var transition = transitions.GetTransitionBetween(previousTrack, playbackSpeed);

            return new MixItem(track, transition);
        }

        PlaybackSpeed GetPlaybackSpeedAtPosition(int index)
        {
            if (items.Count == 0 || index >= items.Count || index < 0)
                return null;

            return items[index].PlaybackSpeed;
        }
    }
}