using System;
using System.Collections.Generic;
using MixPlanner.Configuration;

namespace MixPlanner.IO.Loader
{
    public interface ITagCleanupFactory
    {
        IEnumerable<ITagCleanup> GetCleanups();
    }

    public class TagCleanupFactory : ITagCleanupFactory
    {
        readonly IConfigProvider configProvider;

        public TagCleanupFactory(IConfigProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.configProvider = configProvider;
        }

        public IEnumerable<ITagCleanup> GetCleanups()
        {
            var config = configProvider.Config;

            if (!config.StripMixedInKeyPrefixes)
                yield break;

            yield return new MixedInKeyTagCleanup();
        }
    }
}