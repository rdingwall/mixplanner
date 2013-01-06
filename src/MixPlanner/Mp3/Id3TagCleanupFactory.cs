using System;
using System.Collections.Generic;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface IId3TagCleanupFactory
    {
        IEnumerable<IId3TagCleanup> GetCleanups();
    }

    public class Id3TagCleanupFactory : IId3TagCleanupFactory
    {
        readonly IConfigProvider configProvider;

        public Id3TagCleanupFactory(IConfigProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.configProvider = configProvider;
        }

        public IEnumerable<IId3TagCleanup> GetCleanups()
        {
            var config = configProvider.Config;

            if (!config.StripMixedInKeyPrefixes)
                yield break;

            yield return new MixedInKeyTagCleanup();
        }
    }
}