﻿namespace MixPlanner.Commands
{
    using System;
    using Castle.Windsor;
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.DomainModel;
    using MixPlanner.Events;
    using MixPlanner.ViewModels;

    public sealed class InitializeEmptyMixCommand : CommandBase
    {
        private readonly IWindsorContainer container;

        public InitializeEmptyMixCommand(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        public override void Execute(object parameter)
        {
            // Ensure these are instantiated and registered so they receive the
            // initial mix event.
            container.Resolve<MixSurroundingAreaViewModel>();
            container.Resolve<MixToolBarViewModel>();
            container.Resolve<MainWindowViewModel>();
            container.Resolve<ICurrentMixProvider>();

            var mixFactory = container.Resolve<IMixFactory>();
            IMix mix = mixFactory.Create();
            container.Resolve<IMessenger>().Send(new MixLoadedEvent(mix));
        }
    }
}