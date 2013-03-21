using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(IntelligentInserter))]
    public class IntelligentInserterSpecs
    {
        public class When_finding_the_best_place_to_insert_a_track
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack(HarmonicKey.Key8A, 128);

                    // 0 
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key5A, 128));
                    // 1
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key7A, 128));
                    // 2
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key10A, 128));
                    // 3
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key7A, 150));
                    // 4
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_recommend_the_last_good_position = 
                () => result.InsertIndex.ShouldEqual(2);

            It should_return_the_start_strategy_used = 
                () => result.StartStrategy.ShouldBe(typeof (PerfectFifth));

            It should_return_the_end_strategy_used = 
                () => result.EndStrategy.ShouldBe(typeof (TwoSemitoneEnergyBoost));

            It should_be_a_success = () => result.IsSuccess.ShouldBeTrue();

            It should_not_require_any_adjustment =
                () => result.IncreaseRequired.ShouldBeCloseTo(0, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }

        public class When_the_best_place_to_insert_a_track_was_at_the_end
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack(HarmonicKey.Key8A, 128);

                    // 0 
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key5A, 128));
                    // 1
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key7A, 128));
                    // 2
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 128));
                    // 3
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key7A, 128));
                    // 4
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_recommend_the_last_good_position = 
                () => result.InsertIndex.ShouldEqual(4);

            It should_return_the_start_strategy_used =
                () => result.StartStrategy.ShouldBe(typeof(PerfectFifth));

            It should_not_specify_an_end_strategy =
                () => result.EndStrategy.ShouldBeNull();

            It should_be_a_success = () => result.IsSuccess.ShouldBeTrue();

            It should_not_require_any_adjustment =
                () => result.IncreaseRequired.ShouldBeCloseTo(0, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }

        public class When_the_best_place_to_insert_a_track_was_at_the_start
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack(HarmonicKey.Key8A, 128);

                    // 0 
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key8A, 128));
                    // 1
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key12A, 128));
                    // 2
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key12A, 128));
                    // 3
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key12A, 128));
                    // 4
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_recommend_the_last_good_position = 
                () => result.InsertIndex.ShouldEqual(0);

            It should_not_specify_a_start_strategy =
                () => result.StartStrategy.ShouldBeNull();

            It should_return_the_end_strategy_used =
                () => result.EndStrategy.ShouldBe(typeof(SameKey));

            It should_be_a_success = () => result.IsSuccess.ShouldBeTrue();

            It should_not_require_any_adjustment =
                () => result.IncreaseRequired.ShouldBeCloseTo(0, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }

        public class When_finding_the_best_place_to_insert_a_track_into_an_empty_mix
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack();
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_recommend_adding_it_to_position_zero = 
                () => result.InsertIndex.ShouldEqual(0);

            It should_not_specify_the_start_strategy =
                () => result.StartStrategy.ShouldBeNull();

            It should_not_specify_the_end_strategy =
                () => result.EndStrategy.ShouldBeNull();

            It should_be_a_success = () => result.IsSuccess.ShouldBeTrue();

            It should_not_require_any_adjustment =
                () => result.IncreaseRequired.ShouldBeCloseTo(0, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }

        public class When_there_was_no_good_place_to_insert_the_track
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack(HarmonicKey.Key8A, 128);

                    // 0 
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key11A, 128));
                    // 1
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 128));
                    // 2
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 128));
                    // 3
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 128));
                    // 4
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_not_specify_the_start_strategy =
                () => result.StartStrategy.ShouldBeNull();

            It should_not_specify_the_end_strategy =
                () => result.EndStrategy.ShouldBeNull();

            It should_not_be_a_success = () => result.IsSuccess.ShouldBeFalse();

            It should_not_require_any_adjustment = 
                () => result.IncreaseRequired.ShouldBeCloseTo(0, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }

        public class When_an_adjustment_was_required
        {
            Establish context = () =>
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new IocRegistrations());
                    container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                    inserter = container.Resolve<IIntelligentInserter>();
                    mix = TestMixes.GetEmptyMix();
                    track = TestTracks.GetRandomTrack(HarmonicKey.Key10A, 122);
                    // will be 5A @ 106%

                    // 0 
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key3A, 136));
                    // 1
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key7A, 136));
                    // 2
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 136));
                    // 3
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key9A, 136));
                    // 4c
                }
            };

            Because of = () => result = inserter.GetBestInsertIndex(mix, track);

            It should_be_a_success = () => result.IsSuccess.ShouldBeTrue();

            It should_recommend_the_last_good_position = 
                () => result.InsertIndex.ShouldEqual(1);

            It should_specify_the_start_strategy_used =
                () => result.StartStrategy.ShouldBe(typeof (TwoSemitoneEnergyBoost));

            It should_specify_the_end_strategy_used =
                () => result.EndStrategy.ShouldBe(typeof(TwoSemitoneEnergyBoost));

            It should_specify_the_adjustment_required =
                () => result.IncreaseRequired.ShouldBeCloseTo(0.06, 0.001);

            static InsertResults result;
            static IIntelligentInserter inserter;
            static Mix mix;
            static Track track;
        }
    }
}