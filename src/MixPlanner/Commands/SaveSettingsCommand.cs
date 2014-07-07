namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using MixPlanner.Configuration;
    using MixPlanner.ViewModels;

    public sealed class SaveSettingsCommand : AsyncCommandBase<SettingsWindowViewModel>
    {
        private readonly IConfigProvider configProvider;

        public SaveSettingsCommand(IConfigProvider configProvider)
        {
            if (configProvider == null)
            {
                throw new ArgumentNullException("configProvider");
            }

            this.configProvider = configProvider;
        }

        protected override async Task DoExecute(SettingsWindowViewModel parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            Config config = configProvider.Config;
            UpdateValues(parameter, config);
            await configProvider.SaveAsync(config);
        }

        private static void UpdateValues(SettingsWindowViewModel parameter, Config config)
        {
            config.HarmonicKeyDisplayMode = parameter.HarmonicKeyDisplayMode;
            config.RestrictBpmCompatibility = parameter.RestrictBpmCompatibility;
            config.StripMixedInKeyPrefixes = parameter.StripMixedInKeyPrefixes;
            config.SuggestBpmAdjustedTracks = parameter.SuggestBpmAdjustedTracks;
            config.AutoAdjustBpm = parameter.AutoAdjustBpm;
            config.ParseKeyAndBpmFromFilename = parameter.ParseKeyAndBpmFromFilename;
        }
    }
}