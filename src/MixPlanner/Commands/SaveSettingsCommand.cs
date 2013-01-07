using System;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class SaveSettingsCommand : AsyncCommandBase<SettingsWindowViewModel>
    {
        readonly IConfigStorage configStorage;

        public SaveSettingsCommand(IConfigStorage configStorage)
        {
            if (configStorage == null) throw new ArgumentNullException("configStorage");
            this.configStorage = configStorage;
        }

        protected override async Task DoExecute(SettingsWindowViewModel parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            var config = await configStorage.GetConfigAsync();
            UpdateValues(parameter, config);
            await configStorage.SaveAsync(config);
        }

        static void UpdateValues(SettingsWindowViewModel parameter, Config config)
        {
            config.HarmonicKeyDisplayMode = parameter.HarmonicKeyDisplayMode;
            config.RestrictBpmCompatibility = parameter.RestrictBpmCompatibility;
            config.StripMixedInKeyPrefixes = parameter.StripMixedInKeyPrefixes;
            config.SuggestBpmAdjustedTracks = parameter.SuggestBpmAdjustedTracks;
            config.AutoAdjustBpm = parameter.AutoAdjustBpm;
        }
    }
}