using System;
using System.Windows.Data;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    public interface IHarmonicKeyConverterFactory
    {
        IValueConverter GetConverter();
    }

    public class HarmonicKeyConverterFactory : IHarmonicKeyConverterFactory
    {
        readonly IConfigProvider configProvider;
        readonly TraditionalTextHarmonicKeyConverter traditionalTextConverter;
        readonly TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter;
        readonly CamelotHarmonicKeyCoverter camelotConverter;

        public HarmonicKeyConverterFactory(
            TraditionalTextHarmonicKeyConverter traditionalTextConverter,
            TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter,
            CamelotHarmonicKeyCoverter camelotConverter,
            IConfigProvider configProvider)
        {
            if (traditionalTextConverter == null) throw new ArgumentNullException("traditionalTextConverter");
            if (traditionalSymbolsConverter == null) throw new ArgumentNullException("traditionalSymbolsConverter");
            if (camelotConverter == null) throw new ArgumentNullException("camelotConverter");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.traditionalTextConverter = traditionalTextConverter;
            this.traditionalSymbolsConverter = traditionalSymbolsConverter;
            this.camelotConverter = camelotConverter;
            this.configProvider = configProvider;
        }

        public IValueConverter GetConverter()
        {
            var config = configProvider.Config;

            switch (config.HarmonicKeyDisplayMode)
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