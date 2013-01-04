using System;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

namespace MixPlanner
{
    public interface IDispatcherMessenger : IMessenger
    {
        void SendToUI<TMessage>(TMessage message);
        void SendToUI<TMessage, TTarget>(TMessage message);
        void SendToUI<TMessage>(TMessage message, object token);
    }

    public class DispatcherMessenger : IDispatcherMessenger
    {
        readonly IMessenger messenger;

        public DispatcherMessenger(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
        }

        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            messenger.Register(recipient, action);
        }

        public void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            messenger.Register(recipient, token, action);
        }

        public void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            messenger.Register(recipient, token, receiveDerivedMessagesToo, action);
        }

        public void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            messenger.Register(recipient, receiveDerivedMessagesToo, action);
        }

        public void Send<TMessage>(TMessage message)
        {
            messenger.Send(message);
        }

        public void Send<TMessage, TTarget>(TMessage message)
        {
            messenger.Send<TMessage, TTarget>(message);
        }

        public void Send<TMessage>(TMessage message, object token)
        {
           messenger.Send(message, token);
        }

        public void SendToUI<TMessage>(TMessage message)
        {
            if (DispatcherHelper.UIDispatcher != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() => messenger.Send(message));
            else
                messenger.Send(message);
        }

        public void SendToUI<TMessage, TTarget>(TMessage message)
        {
            if (DispatcherHelper.UIDispatcher != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() => messenger.Send<TMessage, TTarget>(message));
            else
                messenger.Send<TMessage, TTarget>(message);
        }

        public void SendToUI<TMessage>(TMessage message, object token)
        {
            if (DispatcherHelper.UIDispatcher != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() => messenger.Send(message, token));
            else
                messenger.Send(message, token);
        }

        public void Unregister(object recipient)
        {
            messenger.Unregister(recipient);
        }

        public void Unregister<TMessage>(object recipient)
        {
            messenger.Unregister<TMessage>(recipient);
        }

        public void Unregister<TMessage>(object recipient, object token)
        {
            messenger.Unregister<TMessage>(recipient, token);
        }

        public void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            messenger.Unregister(recipient, action);
        }

        public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            messenger.Unregister(recipient, token, action);
        }
    }
}