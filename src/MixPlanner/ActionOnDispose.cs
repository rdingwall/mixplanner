using System;

namespace MixPlanner
{
    public class ActionOnDispose : IDisposable
    {
        readonly Action action;

        public ActionOnDispose(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            this.action = action;
        }

        public void Dispose()
        {
            action();
        }

        ~ActionOnDispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}