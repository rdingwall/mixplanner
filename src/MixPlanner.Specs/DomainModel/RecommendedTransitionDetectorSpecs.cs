using Machine.Specifications;
using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(RecommendedTransitionDetector))]
    public class RecommendedTransitionDetectorSpecs
    {
        public abstract class FixtureBase
        {
            protected static PlaybackSpeed First;
            protected static PlaybackSpeed Second;
            protected static RecommendedTransitionDetector Detector;
            protected static Transition Transition;
            protected static IMixingStrategy Stragegy;

            Because of = () => Transition = Detector.GetTransitionBetween(First, Second);
        }
         
        public class When_a_track_could_be_mixed_using_a_preferred_strategy : FixtureBase
        {
            Establish context =
                () =>
                {
                    First = TestPlaybackSpeeds.RandomPlaybackSpeed();
                    Second = TestPlaybackSpeeds.RandomPlaybackSpeed();

                    var adjuster = MockRepository.GenerateStub<ILimitingPlaybackSpeedAdjuster>();

                    Stragegy = MockRepository.GenerateStub<IMixingStrategy>();
                    Stragegy.Stub(s => s.IsCompatible(First, Second)).Return(true);

                    Detector = new RecommendedTransitionDetector(new[] { Stragegy }, adjuster);
                };

            It should_return_a_transition = () => Transition.ShouldNotBeNull();
            It should_report_the_strategy_used = () => Transition.Strategy.ShouldBeTheSameAs(Stragegy);
            It should_report_the_starting_key = () => Transition.FromKey.ShouldEqual(First.ActualKey);
            It should_report_the_end_key = () => Transition.ToKey.ShouldEqual(Second.ActualKey);
            It should_not_report_any_increase_amount = () => Transition.IncreaseRequired.ShouldEqual(0);
        }

        public class When_a_track_could_not_be_mixed : FixtureBase
        {
            Establish context =
                () =>
                {
                    First = TestPlaybackSpeeds.RandomPlaybackSpeed();
                    Second = TestPlaybackSpeeds.RandomPlaybackSpeed();

                    var adjuster = MockRepository.GenerateStub<ILimitingPlaybackSpeedAdjuster>();

                    Stragegy = MockRepository.GenerateStub<IMixingStrategy>();

                    Detector = new RecommendedTransitionDetector(new[] { Stragegy }, adjuster);
                };

            It should_not_return_a_transition = () => Transition.ShouldBeNull();
        }

        public class When_a_track_needed_to_be_sped_up_and_could_be_mixed_using_a_preferred_strategy : FixtureBase
        {
            Establish context =
                () =>
                {
                    First = TestPlaybackSpeeds.RandomPlaybackSpeed();
                    Second = TestPlaybackSpeeds.RandomPlaybackSpeed();

                    var adjuster = MockRepository.GenerateStub<ILimitingPlaybackSpeedAdjuster>();
                    adjuster.Stub(a => a.GetSuggestedIncrease(First, Second)).Return(0.06);

                    spedUp = Second.AsIncreasedBy(0.06);

                    Stragegy = MockRepository.GenerateStub<IMixingStrategy>();
                    Stragegy.Stub(s => s.IsCompatible(First, spedUp)).Return(true);

                    Detector = new RecommendedTransitionDetector(new[] { Stragegy }, adjuster);
                };

            It should_return_a_transition = () => Transition.ShouldNotBeNull();
            It should_report_the_strategy_used = () => Transition.Strategy.ShouldBeTheSameAs(Stragegy);
            It should_report_the_starting_key = () => Transition.FromKey.ShouldEqual(First.ActualKey);
            It should_report_the_end_key = () => Transition.ToKey.ShouldEqual(spedUp.ActualKey);
            It should_report_the_increase_amount_required = () => Transition.IncreaseRequired.ShouldBeCloseTo(0.06, 0.001);
            static PlaybackSpeed spedUp;
        }

        public class When_a_track_needed_to_be_sped_up_but_could_not_be_mixed : FixtureBase
        {
            Establish context =
                () =>
                {
                    First = TestPlaybackSpeeds.RandomPlaybackSpeed();
                    Second = TestPlaybackSpeeds.RandomPlaybackSpeed();

                    var adjuster = MockRepository.GenerateStub<ILimitingPlaybackSpeedAdjuster>();
                    adjuster.Stub(a => a.GetSuggestedIncrease(First, Second)).Return(0.06);

                    Stragegy = MockRepository.GenerateStub<IMixingStrategy>();

                    Detector = new RecommendedTransitionDetector(new[] { Stragegy }, adjuster);
                };

            It should_not_return_a_transition = () => Transition.ShouldBeNull();
            static PlaybackSpeed spedUp;
        }

    }
}