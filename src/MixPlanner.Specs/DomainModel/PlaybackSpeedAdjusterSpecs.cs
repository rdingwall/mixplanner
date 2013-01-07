﻿using System;
using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(PlaybackSpeedAdjuster))]
    public class PlaybackSpeedAdjusterSpecs
    {
        public abstract class FixtureBase
        {
            protected static double Increase;
            protected static PlaybackSpeed First;
            protected static PlaybackSpeed Second;

            Because of =
                 () => Increase = new PlaybackSpeedAdjuster().GetSuggestedIncrease(First, Second);
        }

        public class When_the_two_playback_speeds_were_within_3_percent_of_each_other : FixtureBase
         {
             Establish context =
                 () =>
                     {
                         First = TestPlaybackSpeeds.PlaybackSpeed(128);
                         Second = TestPlaybackSpeeds.PlaybackSpeed(129);
                     };

             It should_not_suggest_any_change = () => Increase.ShouldBeCloseTo(0, 0.001);
         }

         public class When_the_second_track_needs_to_be_sped_up_by_3_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(133);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(128);
                 };

             It should_recommend_increasing_by_3_percent = () => Increase.ShouldBeCloseTo(0.03, 0.001);
         }

         public class When_the_second_track_needs_to_be_sped_up_by_6_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(136);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(128);
                 };

             It should_recommend_increasing_by_6_percent = () => Increase.ShouldBeCloseTo(0.06, 0.001);
         }

         public class When_the_second_track_needs_to_be_sped_up_more_than_6_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(200);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(128);
                 };

             It should_not_recommend_any_increase = () => Increase.ShouldBeCloseTo(0, 0.001);
         }

         public class When_the_two_playback_speeds_were_within_3_percent_of_each_other_negative : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     Second = TestPlaybackSpeeds.PlaybackSpeed(129);
                     First = TestPlaybackSpeeds.PlaybackSpeed(128);
                 };

             It should_not_suggest_any_change = () => Increase.ShouldBeCloseTo(0, 0.001);
         }

         public class When_the_second_track_needs_to_be_slowed_by_3_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(128);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(133);
                 };

             It should_recommend_decreasing_by_3_percent = () => Increase.ShouldBeCloseTo(-0.03, 0.001);
         }

         public class When_the_second_track_needs_to_be_slowed_by_6_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(128);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(136);
                 };

             It should_recommend_decreasing_by_3_percent = () => Increase.ShouldBeCloseTo(-0.03, 0.001);
         }

         public class When_the_second_track_needs_to_be_slowed_more_than_6_percent : FixtureBase
         {
             Establish context =
                 () =>
                 {
                     First = TestPlaybackSpeeds.PlaybackSpeed(128);
                     Second = TestPlaybackSpeeds.PlaybackSpeed(200);
                 };

             It should_not_recommend_any_decrease = () => Increase.ShouldBeCloseTo(0, 0.001);
         }
    }
}