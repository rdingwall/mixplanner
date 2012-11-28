using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(TransitionTypeDetector))]
    public class TransitionTypeDetecterSpecs
    {
         public class When_detecting_the_type_of_transition_between_two_tracks
         {
             Establish context =
                 () =>
                     {
                         detector = new TransitionTypeDetector(Strategies.AllStrategies);
                     };

             Because of = 
                 () => transition = detector.GetTransitionBetween(
                     TestTracks.Get(HarmonicKey.Key1A), 
                     TestTracks.Get(HarmonicKey.Key2A));

             It should_give_the_correct_strategy = 
                 () => transition.Strategy.ShouldBe(typeof(IncrementOne));

             It should_return_the_first_key = () => transition.FromKey.ShouldEqual(HarmonicKey.Key1A);

             It should_return_the_second_key = () => transition.ToKey.ShouldEqual(HarmonicKey.Key2A);

             static TransitionTypeDetector detector;
             static Transition transition;
         }
    }
}