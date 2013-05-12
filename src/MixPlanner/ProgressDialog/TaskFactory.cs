using System;
using System.Threading;
using System.Threading.Tasks;

namespace MixPlanner.ProgressDialog
{
    public delegate Task TaskFactory(IProgress<string> progress,
                                     CancellationToken cancellationToken);

    public delegate Task<T> TaskFactory<T>(IProgress<string> progress,
                                     CancellationToken cancellationToken);
}