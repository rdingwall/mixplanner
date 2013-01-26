﻿using System;
using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.Loader;

namespace MixPlanner.Specs.Loader
{
    // Full end-to-end integration tests for loading various file types.
    [Subject(typeof(TrackLoader))]
    public class TrackLoaderSpecs
    {
        public class when_reading_tags_from_a_track_tagged_by_audacity : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("audacity.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key7A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Hardwell");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(128);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Progressive House");

            It should_get_the_filename = 
                () => Track.Filename.ShouldEndWith("audacity.mp3");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_track_tagged_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("mixed_in_key_4.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key7A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Hardwell");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(128);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Progressive House");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("mixed_in_key_4.mp3");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_full_track_tagged_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => 
                Track = Loader.LoadAsync("7A - 128 - 3505135_Three Triangles_Original Club Mix.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key7A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Hardwell");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(128);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Progressive House");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("7A - 128 - 3505135_Three Triangles_Original Club Mix.mp3");

            It should_get_the_images =
                () => Track.Images.ShouldNotBeNull();
        }

        // This was generated by an old version of the Mixed in Key algorithm,
        // if you run the track again now it reports 1B. (No wonder I could
        // never mix it 2 years ago)
        public class when_reading_tags_from_a_full_track_tagged_with_two_keys_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => 
                Track = Loader.LoadAsync("1A or 11A - 132 - 1279464_Barra_Extended Mix.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key1A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Cosmic Gate");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Barra - Extended Mix");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(132);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Black Hole Recordings");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2010");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Trance");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("1A or 11A - 132 - 1279464_Barra_Extended Mix.mp3");

            It should_get_the_images =
                () => Track.Images.ShouldNotBeNull();
        }

        public class when_reading_tags_from_a_track_with_only_id3v1_tag : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("id3v1_only.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Unknown);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Aphex Twin");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Bit");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(Double.NaN);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("1995");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Electronic");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("id3v1_only.mp3");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_track_with_no_initial_key_or_bpm : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("id3v2_no_key_or_bpm.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Unknown);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Method Man");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Say (Call Out)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(Double.NaN);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Def Jam Records");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2006");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Rap");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("id3v2_no_key_or_bpm.mp3");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_wav_tagged_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("9A - 127 - 3813814_Your Love_Mark Knight Remix.wav").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key9A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual(TrackDefaults.UnknownArtist);

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("3813814_Your Love_Mark Knight Remix");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(127);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("9A - 127 - 3813814_Your Love_Mark Knight Remix.wav");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_corrupt_mp3_with_key_and_bpm_in_filename : FixtureBase
        {
            Because of = () => Track = Loader.LoadAsync("12A - 128 - corrupt.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key12A);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual(TrackDefaults.UnknownArtist);

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("corrupt");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(128);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("12A - 128 - corrupt.mp3");

            It should_not_return_any_images =
                () => Track.Images.ShouldBeNull();
        }

        public class when_reading_tags_from_a_beatport_aiff : FixtureBase
        {
            Because of = () =>
                Track = Loader.LoadAsync("3504338_Harlem Shake_Original Mix.aiff").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Unknown);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Baauer");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Harlem Shake (Original Mix)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(93);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Mad Decent");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Electro House");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("3504338_Harlem Shake_Original Mix.aiff");

            It should_get_the_images =
                () => Track.Images.ShouldNotBeNull();
        }

        public class when_reading_tags_from_a_beatport_aiff_converted_to_m4a : FixtureBase
        {
            Because of = () =>
                Track = Loader.LoadAsync("07 Harlem Shake (Original Mix).m4a").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Unknown);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Baauer");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("Harlem Shake (Original Mix)");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(93);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("Mad Decent");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Electro House");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("07 Harlem Shake (Original Mix).m4a");

            It should_get_the_images =
                () => Track.Images.ShouldNotBeNull();
        }
        
        // NB the second key here is ficticious (added it manually)
        public class when_reading_tags_from_a_mp3_tagged_with_two_keys_by_rapid_evolution_3 : FixtureBase
        {
            Because of = () =>
                Track = Loader.LoadAsync("1896735_The Mule_Album Mix.mp3").Result;

            It should_get_the_correct_key =
                () => Track.OriginalKey.ShouldEqual(HarmonicKey.Key6B);

            It should_get_the_correct_artist =
                () => Track.Artist.ShouldEqual("Orjan Nilsen");

            It should_get_the_correct_title =
                () => Track.Title.ShouldEqual("The Mule - Album Mix");

            It should_get_the_correct_bpm =
                () => Track.OriginalBpm.ShouldEqual(128);

            It should_get_the_correct_publisher =
                () => Track.Label.ShouldEqual("");

            It should_get_the_correct_year =
                () => Track.Year.ShouldEqual("2011");

            It should_get_the_correct_genre =
                () => Track.Genre.ShouldEqual("Trance");

            It should_get_the_filename =
                () => Track.Filename.ShouldEndWith("1896735_The Mule_Album Mix.mp3");

            It should_get_the_images =
                () => Track.Images.ShouldNotBeNull();
        }

        public abstract class FixtureBase
        {
            Establish context =
                () =>
                    {
                        container = new WindsorContainer();
                        container.Install(new IocRegistrations());
                        container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                        Loader = container.Resolve<ITrackLoader>();
                    };

            Cleanup after = () => container.Dispose();

            It should_return_a_track = () => Track.ShouldNotBeNull();

            protected static ITrackLoader Loader;
            protected static Track Track;
            static IWindsorContainer container;
        }
    }
}