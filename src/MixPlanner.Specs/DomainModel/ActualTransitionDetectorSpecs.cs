﻿using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(ActualTransitionDetector))]
    public class ActualTransitionDetectorSpecs
    {
         public class When_detecting_the_type_of_transition_between_two_tracks
         {
             Establish context =
                 () =>
                     {
                         detector = new ActualTransitionDetector(TestMixingStrategies.AllStrategies);
                     };

             Because of =
                 () => transition = detector.GetTransitionBetween(
                     new PlaybackSpeed(HarmonicKey.Key1A, 128),
                     new PlaybackSpeed(HarmonicKey.Key2A, 128));

             It should_give_the_correct_strategy = 
                 () => transition.Strategy.ShouldBe(typeof(AdvanceOne));

             It should_return_the_first_key = () => transition.FromKey.ShouldEqual(HarmonicKey.Key1A);

             It should_return_the_second_key = () => transition.ToKey.ShouldEqual(HarmonicKey.Key2A);

             static ActualTransitionDetector detector;
             static Transition transition;
         }

         public class When_detecting_the_type_of_transition_for_the_opening_intro_track
         {
             Establish context =
                 () =>
                 {
                     detector = new ActualTransitionDetector(TestMixingStrategies.AllStrategies);
                 };

             Because of =
                 () => transition = detector.GetTransitionBetween(
                     null,
                     new PlaybackSpeed(HarmonicKey.Key2A, 128));

             It should_give_the_correct_strategy =
                 () => transition.Strategy.ShouldBeNull();

             It should_return_the_first_key = 
                 () => transition.FromKey.ShouldBeNull();

             It should_return_the_second_key = 
                 () => transition.ToKey.ShouldEqual(HarmonicKey.Key2A);

             static ActualTransitionDetector detector;
             static Transition transition;
         }

         public class When_detecting_the_type_of_transition_for_the_closing_outro_track
         {
             Establish context =
                 () =>
                 {
                     detector = new ActualTransitionDetector(TestMixingStrategies.AllStrategies);
                 };

             Because of =
                 () => transition = detector.GetTransitionBetween(
                     new PlaybackSpeed(HarmonicKey.Key2A, 128),
                     null);

             It should_give_the_correct_strategy =
                 () => transition.Strategy.ShouldBeNull();

             It should_return_the_first_key =
                 () => transition.FromKey.ShouldEqual(HarmonicKey.Key2A);

             It should_return_the_second_key =
                 () => transition.ToKey.ShouldBeNull();

             static ActualTransitionDetector detector;
             static Transition transition;
         }
    }
}