using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(TransitionDescriptionConverter))]
    public class TransitionDescriptionConverterSpecs
    {
         public class When_the_previous_key_was_null
         {
             Establish context =
                 () => transition = Transition.Intro(HarmonicKey.RandomKey());

             Because of = () => description = new TransitionDescriptionConverter()
                 .Convert(transition, null, null, null);

             It should_be_described_as_an_intro_mix = () => description.ShouldEqual(">>> Intro");

             static Transition transition;
             static object description;
         }

         public class When_the_following_key_was_null
         {
             Establish context =
                 () => transition = Transition.Outro(HarmonicKey.RandomKey());

             Because of = () => description = new TransitionDescriptionConverter()
                 .Convert(transition, null, null, null);

             It should_be_described_as_an_outro_mix = () => description.ShouldEqual(">>> Outro");

             static Transition transition;
             static object description;
         }

         public class When_there_was_a_strategy
         {
             Establish context =
                 () =>
                 {
                     var strategy = MockRepository.GenerateMock<IMixingStrategy>();
                     strategy.Stub(s => s.Description).Return("Test Strategy");
                     transition = new Transition(HarmonicKey.RandomKey(), HarmonicKey.RandomKey(),
                                                 strategy);
                 };

             Because of =
                 () => description = new TransitionDescriptionConverter()
                                         .Convert(transition, null, null, null);

             It should_use_the_strategys_name = 
                 () => description.ShouldEqual(">>> Test Strategy");

             static Transition transition;
             static object description;
         }
    }
}