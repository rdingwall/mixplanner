namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.DomainModel;
    using MixPlanner.Events;

    public interface IGuardUnsavedChangesService
    {
        void GuardUnsavedChanges(Action action);

        void GuardUnsavedChanges(Action action, Action cancel);
    }

    public sealed class GuardUnsavedChangesService : IGuardUnsavedChangesService
    {
        private readonly IDispatcherMessenger messenger;
        private readonly ICurrentMixProvider mixProvider;
        private readonly SaveMixAsCommand saveAsCommand;
        private bool isDirty;

        public GuardUnsavedChangesService(
            IDispatcherMessenger messenger,
            ICurrentMixProvider mixProvider,
            SaveMixAsCommand saveAsCommand)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }

            if (saveAsCommand == null)
            {
                throw new ArgumentNullException("saveAsCommand");
            }

            this.messenger = messenger;
            this.mixProvider = mixProvider;
            this.saveAsCommand = saveAsCommand;

            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
            messenger.Register<MixDirtyEvent>(this, OnMixDirty);
            messenger.Register<MixSavedEvent>(this, OnMixSaved);
        }

        public void GuardUnsavedChanges(Action action)
        {
            GuardUnsavedChanges(action, delegate { });
        }

        public void GuardUnsavedChanges(Action action, Action cancel)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (cancel == null)
            {
                throw new ArgumentNullException("cancel");
            }

            if (!isDirty)
            {
                action();
                return;
            }

            var message =
                new DialogMessage(this, "Save tracks?", m => OnResult(m, action, cancel))
                    {
                        Button = MessageBoxButton.YesNoCancel,
                        Icon = MessageBoxImage.Warning,
                        Caption = "MixPlanner"
                    };

            messenger.SendToUI(message);
        }

        private async void OnResult(MessageBoxResult result, Action action, Action cancel)
        {
            switch (result)
            {
                case MessageBoxResult.No: // don't save
                    break;

                case MessageBoxResult.Yes: // save
                    await saveAsCommand.ExecuteAsync(mixProvider.GetCurrentMix());
                    break;

                default: // cancel
                    cancel();
                    return;
            }

            await Task.Run(action);
        }

        private void OnMixDirty(MixDirtyEvent obj)
        {
            isDirty = true;
        }

        private void OnMixSaved(MixSavedEvent obj)
        {
            isDirty = false;
        }

        private void OnMixLoaded(MixLoadedEvent obj)
        {
            isDirty = false;
        }
    }
}