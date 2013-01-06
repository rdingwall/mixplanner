using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface IId3TagCleanupFactory
    {
        IEnumerable<IId3TagCleanup> GetCleanups();
    }

    public class Id3TagCleanupFactory : IId3TagCleanupFactory
    {
        readonly IConfigurationProvider configProvider;

        public Id3TagCleanupFactory(IConfigurationProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.configProvider = configProvider;
        }

        public IEnumerable<IId3TagCleanup> GetCleanups()
        {
            var config = configProvider.Configuration;

            if (!config.StripMixedInKeyPrefixes)
                yield break;

            yield return new MixedInKeyTagCleanup();
        }
    }
}