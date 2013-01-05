using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(RecommendationFactorCalculator))]
    public class RecommendationFactorCalculatorSpecs
    {
        public class When_getting_the_recommendation_factor_for_a_track
        {
            static double factor;

            Establish context =
                () =>
                    {
                        current = TestMixes.GetRandomMix().Items.First();
                        next = TestTracks.GetRandomTrack();

                        var strategy1 = MockRepository.GenerateMock<IMixingStrategy>();
                        var strategy2 = MockRepository.GenerateMock<IMixingStrategy>();

                        strategy2
                            .Stub(s => s.IsCompatible(current.PlaybackSpeed, next.GetDefaultPlaybackSpeed()))
                            .Return(true);

                        calculator = new RecommendationFactorCalculator(new[] {strategy1, strategy2});
                    };

            Because of = () => factor = calculator.GetRecommendationFactor(current, next);
            
            It should_return_the_expected_recommedation_factor_greater_than_zero =
                () => factor.ShouldBeCloseTo(0.666666666666667);

            static RecommendationFactorCalculator calculator;
            static MixItem current;
            static Track next;
        }

        public class When_it_was_recommended_by_the_first_strategy
        {
            static double factor;

            Establish context =
                () =>
                {
                    current = TestMixes.GetRandomMix().Items.First();
                    next = TestTracks.GetRandomTrack();

                    var strategy1 = MockRepository.GenerateMock<IMixingStrategy>();
                    var strategy2 = MockRepository.GenerateMock<IMixingStrategy>();

                    strategy1
                        .Stub(s => s.IsCompatible(current.PlaybackSpeed, next.GetDefaultPlaybackSpeed()))
                        .Return(true);

                    calculator = new RecommendationFactorCalculator(new[] { strategy1, strategy2 });
                };

            Because of = () => factor = calculator.GetRecommendationFactor(current, next);

            It should_return_1 =
                () => factor.ShouldBeCloseTo(1);

            static RecommendationFactorCalculator calculator;
            static MixItem current;
            static Track next;
        }

        public class When_it_was_not_recommended
        {
            static double factor;

            Establish context =
                () =>
                {
                    current = TestMixes.GetRandomMix().Items.First();
                    next = TestTracks.GetRandomTrack();

                    var strategy1 = MockRepository.GenerateMock<IMixingStrategy>();
                    var strategy2 = MockRepository.GenerateMock<IMixingStrategy>();

                    calculator = new RecommendationFactorCalculator(new[] { strategy1, strategy2 });
                };

            Because of = () => factor = calculator.GetRecommendationFactor(current, next);

            It should_return_zero =
                () => factor.ShouldBeCloseTo(0);

            static RecommendationFactorCalculator calculator;
            static MixItem current;
            static Track next;
        }
    }
}