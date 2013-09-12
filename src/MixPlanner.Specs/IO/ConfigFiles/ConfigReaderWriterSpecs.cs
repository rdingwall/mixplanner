using System;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.IO.ConfigFiles;

namespace MixPlanner.Specs.IO.ConfigFiles
{
    [Subject("Config reader/writer")]
    public class ConfigReaderWriterSpecs
    {
         public class When_saving_and_loading_the_config
         {

             Establish context =
                 () =>
                     {
                         filename = TestDirectories.ConfigFile;

                         originalConfig = new Config
                                              {
                                                  AutoAdjustBpm = true,
                                                  HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.TraditionalWithSymbols,
                                                  ParseKeyAndBpmFromFilename = false,
                                                  RestrictBpmCompatibility = true,
                                                  StripMixedInKeyPrefixes = true,
                                                  SuggestBpmAdjustedTracks = false
                                              };

                         writer = new ConfigWriter();
                         writer.WriteAsync(originalConfig, filename).Wait();

                         originalConfig.RestrictBpmCompatibility = !originalConfig.RestrictBpmCompatibility;

                         writer.WriteAsync(originalConfig, filename).Wait();

                         reader = new ConfigReader();
                     };

             Because of = () => config = reader.ReadAsync(filename).Result;

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

             static IConfigWriter writer;
             static Config originalConfig;
             static Config config;
             static IConfigReader reader;
             static string filename;
         }

         public class When_loading_a_non_existing_config
         {
             Establish context = () => reader = new ConfigReader();

             Because of = () => error = Catch.Exception(() => config = reader.ReadAsync(@"\\nonexisting\dir\dummy.settings").Result);

             It should_not_throw_any_error = () => error.ShouldBeNull();

             It should_return_a_null_config = () => config.ShouldBeNull();
             
             static Config config;
             static Exception error;
             static IConfigReader reader;
         }

         public class When_saving_to_a_non_existing_directory
         {
             Establish context = () => writer = new ConfigWriter();

             Because of = () => error = Catch.Exception(() => writer.WriteAsync(new Config(), @"\\nonexisting\dir\dummy.settings").Wait());

             It should_not_throw_any_error = () => error.ShouldBeNull();

             static IConfigWriter writer;
             static Exception error;
         }
    }
}