using System;
using System.Collections.Generic;
using System.Windows.Data;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    public interface IHarmonicKeyConverterFactory
    {
        IValueConverter GetConverter();
        IEnumerable<IValueConverter> GetAllConverters();
    }

    public class HarmonicKeyConverterFactory : IHarmonicKeyConverterFactory
    {
        readonly IConfigProvider configProvider;
        readonly TraditionalTextHarmonicKeyConverter traditionalTextConverter;
        readonly TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter;
        readonly KeyCodeHarmonicKeyCoverter keyCodeConverter;
        readonly Id3v2TkeyHarmonicKeyConverter id3V2TkeyConverter;
        readonly OpenKeyNotationHarmonicKeyConverter openKeyNotationConverter;
        readonly BeatportHarmonicKeyConverter beatportHarmonicKeyConverter;

        public HarmonicKeyConverterFactory(
            TraditionalTextHarmonicKeyConverter traditionalTextConverter,
            TraditionalSymbolsHarmonicKeyConverter traditionalSymbolsConverter,
            KeyCodeHarmonicKeyCoverter keyCodeConverter,
            Id3v2TkeyHarmonicKeyConverter id3v2TkeyConverter,
            OpenKeyNotationHarmonicKeyConverter openKeyNotationConverter,
            BeatportHarmonicKeyConverter beatportHarmonicKeyConverter,
            IConfigProvider configProvider)
        {
            if (traditionalTextConverter == null) throw new ArgumentNullException("traditionalTextConverter");
            if (traditionalSymbolsConverter == null) throw new ArgumentNullException("traditionalSymbolsConverter");
            if (keyCodeConverter == null) throw new ArgumentNullException("keyCodeConverter");
            if (id3v2TkeyConverter == null) throw new ArgumentNullException("id3v2TkeyConverter");
            if (openKeyNotationConverter == null) throw new ArgumentNullException("openKeyNotationConverter");
            if (beatportHarmonicKeyConverter == null) throw new ArgumentNullException("beatportHarmonicKeyConverter");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.traditionalTextConverter = traditionalTextConverter;
            this.traditionalSymbolsConverter = traditionalSymbolsConverter;
            this.keyCodeConverter = keyCodeConverter;
            id3V2TkeyConverter = id3v2TkeyConverter;
            this.openKeyNotationConverter = openKeyNotationConverter;
            this.configProvider = configProvider;
            this.beatportHarmonicKeyConverter = beatportHarmonicKeyConverter;
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

                case HarmonicKeyDisplayMode.Id3Tkey:
                    return id3V2TkeyConverter;

                case HarmonicKeyDisplayMode.OpenKeyNotation:
                    return openKeyNotationConverter;

                default:
                case HarmonicKeyDisplayMode.KeyCode:
                    return keyCodeConverter;
            }
        }

        public IEnumerable<IValueConverter> GetAllConverters()
        {
            return new IValueConverter[]
                       {
                           keyCodeConverter,
                           traditionalSymbolsConverter,
                           traditionalTextConverter,
                           id3V2TkeyConverter,
                           openKeyNotationConverter,
                           beatportHarmonicKeyConverter
                       };
        }
    }
}