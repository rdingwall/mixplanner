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
        }

        MixItem CreateItem(Track track, int insertIndex)
        {
            var previousTrack = GetTrackAtPosition(insertIndex - 1);
            var transition = transitions.GetTransitionBetween(previousTrack, track);

            return new MixItem(track, transition);
        }

        Track GetTrackAtPosition(int index)
        {
            if (items.Count == 0 || index >= items.Count || index < 0)
                return null;

            return items[index].Track;
        }
    }
}