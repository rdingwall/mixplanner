using System;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.Storage;

namespace MixPlanner.Specs.Storage
{
    [Subject(typeof(JsonFileConfigStorage))]
    public class JsonFileConfigStorageSpecs
    {
         public class When_saving_and_loading_the_config
         {
             Establish context =
                 () =>
                     {

                         storage = new JsonFileConfigStorage(TestDirectories.Library.Path);

                         originalConfig = new Config
                                              {
                                                  AutoAdjustBpm = true,
                                                  HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.TraditionalWithSymbols,
                                                  ParseKeyAndBpmFromFilename = false,
                                                  RestrictBpmCompatibility = true,
                                                  StripMixedInKeyPrefixes = true,
                                                  SuggestBpmAdjustedTracks = false
                                              };

                         storage.SaveAsync(originalConfig).Wait();

                         originalConfig.RestrictBpmCompatibility = !originalConfig.RestrictBpmCompatibility;

                         storage.SaveAsync(originalConfig).Wait();
                     };

             Because of = () => config = storage.LoadConfigAsync().Result;

             It should_correctly_retrieve_the_auto_adjust_bpm_setting =
                 () => config.AutoAdjustBpm.ShouldEqual(originalConfig.AutoAdjustBpm);

             It should_correctly_retrieve_the_harmonic_key_display_mode =
                 () => config.HarmonicKeyDisplayMode.ShouldEqual(originalConfig.HarmonicKeyDisplayMode);

             It should_correctly_retrieve_the_parse_key_and_bpm_from_filename =
                 () => config.ParseKeyAndBpmFromFilename.ShouldEqual(originalConfig.ParseKeyAndBpmFromFilename);

             It should_correctly_retrieve_the_restrict_bpm_compatibility =
                 () => config.RestrictBpmCompatibility.ShouldEqual(originalConfig.RestrictBpmCompatibility);

             It should_correctly_retrieve_the_strip_mixed_in_key_prefixes =
                 () => config.StripMixedInKeyPrefixes.ShouldEqual(originalConfig.StripMixedInKeyPrefixes);

             It should_correctly_retrieve_the_suggest_bpm_adjusted_tracks =
                 () => config.SuggestBpmAdjustedTracks.ShouldEqual(originalConfig.SuggestBpmAdjustedTracks);

             static IConfigStorage storage;
             static Config originalConfig;
             static Config config;
         }

         public class When_loading_a_non_existing_config
         {
             Establish context =
                 () => storage = new JsonFileConfigStorage(@"\\nonexisting\dir");

             Because of = () => error = Catch.Exception(() => config = storage.LoadConfigAsync().Result);

             It should_not_throw_any_error = () => error.ShouldBeNull();

             It should_return_a_null_config = () => config.ShouldBeNull();
             
             static IConfigStorage storage;
             static Config originalConfig;
             static Config config;
             static Exception error;
         }

         public class When_saving_to_a_non_existing_directory
         {
             Establish context =
                 () => storage = new JsonFileConfigStorage(@"\\nonexisting\dir");

             Because of = () => error = Catch.Exception(() => storage.SaveAsync(new Config()).Wait());

             It should_not_throw_any_error = () => error.ShouldBeNull();

             static IConfigStorage storage;
             static Exception error;
         }
    }
}