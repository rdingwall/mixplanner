﻿using System;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public interface IGuardUnsavedChangesService
    {
        void GuardUnsavedChanges(Action action);
    }

    public class GuardUnsavedChangesService : IGuardUnsavedChangesService
    {
        readonly IDispatcherMessenger messenger;
        readonly ICurrentMixProvider mixProvider;
        readonly SaveMixAsCommand saveAsCommand;
        bool isDirty;

        public GuardUnsavedChangesService(
            IDispatcherMessenger messenger,
            ICurrentMixProvider mixProvider,
            SaveMixAsCommand saveAsCommand)
        {
            if (mixProvider == null) throw new ArgumentNullException("mixProvider");
            if (saveAsCommand == null) throw new ArgumentNullException("saveAsCommand");
            this.messenger = messenger;
            this.mixProvider = mixProvider;
            this.saveAsCommand = saveAsCommand;

            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
            messenger.Register<MixDirtyEvent>(this, OnMixDirty);
            messenger.Register<MixSavedEvent>(this, OnMixSaved);
        }

        public void GuardUnsavedChanges(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (!isDirty)
            {
                action();
                return;
            }

            var message =
                new DialogMessage(this, "Save tracks?", m => OnResult(m, action))
                    {
                        Button = MessageBoxButton.YesNoCancel,
                        Icon = MessageBoxImage.Warning,
                        Caption = "MixPlanner"
                    };

            messenger.SendToUI(message);
        }

        async void OnResult(MessageBoxResult result, Action action)
        {
            switch (result)
            {
                case MessageBoxResult.No: // don't save
                    break;

                case MessageBoxResult.Yes: // save
                    await saveAsCommand.ExecuteAsync(mixProvider.GetCurrentMix());
                    break;

                default: // cancel
                    return;
            }

            await Task.Run(action);
        }

        void OnMixDirty(MixDirtyEvent obj)
        {
            isDirty = true;
        }

        void OnMixSaved(MixSavedEvent obj)
        {
            isDirty = false;
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            isDirty = false;
        }
    }
}