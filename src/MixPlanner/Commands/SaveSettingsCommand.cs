using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class SaveSettingsCommand : AsyncCommandBase<SettingsWindowViewModel>
    {
        readonly IConfigurationStorage configurationStorage;

        public SaveSettingsCommand(IConfigurationStorage configurationStorage)
        {
            if (configurationStorage == null) throw new ArgumentNullException("configurationStorage");
            this.configurationStorage = configurationStorage;
        }

        protected override async Task DoExecute(SettingsWindowViewModel parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            var configuration = await configurationStorage.GetConfiguration();
            UpdateValues(parameter, configuration);
            await configurationStorage.Save(configuration);
        }

        static void UpdateValues(SettingsWindowViewModel parameter, Configuration configuration)
        {
            configuration.HarmonicKeyDisplayMode = parameter.HarmonicKeyDisplayMode;
            configuration.RestrictBpmCompatibility = parameter.RestrictBpmCompatibility;
            configuration.StripMixedInKeyPrefixes = parameter.StripMixedInKeyPrefixes;
        }
    }
}