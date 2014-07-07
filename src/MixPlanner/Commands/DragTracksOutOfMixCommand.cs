﻿namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;

    public sealed class DragTracksOutOfMixCommand : CommandBase<DropInfo>
    {
        private readonly ICurrentMixProvider mixProvider;
        private readonly RemoveTracksFromMixCommand removeCommand;

        public DragTracksOutOfMixCommand(ICurrentMixProvider mixProvider, RemoveTracksFromMixCommand removeCommand)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }

            if (removeCommand == null)
            {
                throw new ArgumentNullException("removeCommand");
            }

            this.mixProvider = mixProvider;
            this.removeCommand = removeCommand;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null)
            {
                return false;
            }

            IMix mix = mixProvider.GetCurrentMix();

            if (mix.IsLocked)
            {
                return false;
            }

            return parameter.Data is IMixItem || IsCollection(parameter.Data);
        }

        protected override void Execute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as IMixItem;

            if (sourceItem != null)
            {
                removeCommand.Execute(new[] { sourceItem });
            }

            var sourceItems = parameter.Data as IEnumerable<IMixItem>;

            if (sourceItems != null && sourceItems.Any())
            {
                removeCommand.Execute(sourceItems);
            }
        }

        private static bool IsCollection(object obj)
        {
            var items = obj as IEnumerable<IMixItem>;
            return items != null && items.Any();
        }
    }
}