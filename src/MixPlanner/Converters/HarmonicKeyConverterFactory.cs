using System;
using System.Windows.Data;
using Castle.Windsor;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    public interface IHarmonicKeyConverterFactory
    {
        IValueConverter GetConverter();
    }

    public class HarmonicKeyConverterFactory : IHarmonicKeyConverterFactory
    {
        readonly IConfigurationProvider provider;
        readonly TraditionalTextHarmonicKeyConverter traditionalTextConverter;
        readonly TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter;
        readonly CamelotHarmonicKeyCoverter camelotConverter;

        public HarmonicKeyConverterFactory(
            TraditionalTextHarmonicKeyConverter traditionalTextConverter,
            TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter,
            CamelotHarmonicKeyCoverter camelotConverter,
            IConfigurationProvider provider)
        {
            if (traditionalTextConverter == null) throw new ArgumentNullException("traditionalTextConverter");
            if (traditionalSymbolsConverter == null) throw new ArgumentNullException("traditionalSymbolsConverter");
            if (camelotConverter == null) throw new ArgumentNullException("camelotConverter");
            if (provider == null) throw new ArgumentNullException("provider");
            this.traditionalTextConverter = traditionalTextConverter;
            this.traditionalSymbolsConverter = traditionalSymbolsConverter;
            this.camelotConverter = camelotConverter;
            this.provider = provider;
        }

        public IValueConverter GetConverter()
        {
            var configuration = provider.Configuration;

            switch (configuration.HarmonicKeyDisplayMode)
            {
                case HarmonicKeyDisplayMode.TraditionalWithText:
                    return traditionalTextConverter;

                case HarmonicKeyDisplayMode.TraditionalWithSymbols:
                    return traditionalSymbolsConverter;

                default:
                case HarmonicKeyDisplayMode.Camelot:
                    return camelotConverter;
            }
        }
    }
}