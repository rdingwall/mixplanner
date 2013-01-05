using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MoreLinq;

namespace MixPlanner.Commands
{
    public class GetRecommendationsCommand : CommandBase<IEnumerable<MixItem>>
    {
        readonly IDispatcherMessenger messenger;
        readonly ITrackLibrary trackLibrary;

        public GetRecommendationsCommand(
            IDispatcherMessenger messenger,
            ITrackLibrary trackLibrary)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (trackLibrary == null) throw new ArgumentNullException("trackLibrary");
            this.messenger = messenger;
            this.trackLibrary = trackLibrary;
        }

        protected override bool CanExecute(IEnumerable<MixItem> parameter)
        {
            return parameter.Count() == 1;
        }

        protected override void Execute(IEnumerable<MixItem> parameter)
        {
            if (!CanExecute(parameter))
                return;

            var recommendations = trackLibrary.GetRecommendations(parameter.First());

            recommendations.Select(t => new TrackRecommendedEvent(t.Item1, t.Item2))
                           .ForEach(e => messenger.SendToUI(e));
        }
    }
}