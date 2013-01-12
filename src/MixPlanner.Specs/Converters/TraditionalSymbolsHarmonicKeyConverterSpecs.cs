﻿using System.Windows.Data;
using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(TraditionalSymbolsHarmonicKeyConverter))]
    public class TraditionalSymbolsHarmonicKeyConverterSpecs
    {
         public class When_mapping_from_keycode_to_traditional
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("B♭ Major");

             static IValueConverter converter;
         }

         public class When_mapping_an_unknown_key_to_traditional
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

             static IValueConverter converter;
         }

         public class When_mapping_from_traditional_to_keycode
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("B♭ Major", null, null, null).ShouldEqual(HarmonicKey.Key6B);

             static IValueConverter converter;
         }

         public class When_mapping_an_unknown_traditional_key_to_keycode
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("sadasd", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static IValueConverter converter;
         }

         public class When_mapping_an_enharmonic_traditional_key_to_keycode
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("C♯ Minor", null, null, null).ShouldEqual(HarmonicKey.Key12A);

             static IValueConverter converter;
         }
    }
}