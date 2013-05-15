using System;
using System.Collections.Generic;
using Castle.Core;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.Controllers
{
    // must be IStartable else Windsor won't ever instantiate this
    public class RecommendationsController : IStartable
    {
        readonly IMessenger messenger;
        readonly GetRecommendationsCommand getRecommendationsCommand;
        readonly ClearRecommendationsCommand clearRecommendationsCommand;
        bool isRecommendationsEnabled;
        IEnumerable<IMixItem> selectedItems;

        public RecommendationsController(
            IMessenger messenger,
            GetRecommendationsCommand getRecommendationsCommand,
            ClearRecommendationsCommand clearRecommendationsCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (getRecommendationsCommand == null) throw new ArgumentNullException("getRecommendationsCommand");
            if (clearRecommendationsCommand == null) throw new ArgumentNullException("clearRecommendationsCommand");
            
            isRecommendationsEnabled = true;

            this.messenger = messenger;
            this.getRecommendationsCommand = getRecommendationsCommand;
            this.clearRecommendationsCommand = clearRecommendationsCommand;
        }

        public void Start()
        {
            messenger.Register<MixItemSelectionChangedEvent>(this, OnSelectionChanged);
            messenger.Register<RecommendationsDisabledEvent>(this, _ => isRecommendationsEnabled = false);
            messenger.Register<RecommendationsEnabledEvent>(this, _ => EnableRecommendations());
        }

        public void Stop()
        {
            messenger.Unregister(this);
        }

        void OnSelectionChanged(MixItemSelectionChangedEvent e)
        {
            if (!isRecommendationsEnabled)
                return;

            clearRecommendationsCommand.Execute(null);
            selectedItems = e.SelectedItems;
            getRecommendationsCommand.Execute(selectedItems);
        }

        void EnableRecommendations()
        {
            isRecommendationsEnabled = true;
            clearRecommendationsCommand.Execute(null);
            getRecommendationsCommand.Execute(selectedItems);
        }
    }
}