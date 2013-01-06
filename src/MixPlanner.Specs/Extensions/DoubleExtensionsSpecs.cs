using System;
using Machine.Specifications;

namespace MixPlanner.Specs.Extensions
{
    [Subject(typeof(DoubleExtensions))]
    public class DoubleExtensionsSpecs
    {
        public class When_it_was_just_above_the_nearest_interval
        {
            static double rounded;

            Because of = () => rounded = 0.61.RoundToNearest(0.3);

            It should_round_down = () => rounded.ShouldBeCloseTo(0.6, 000.1);
        }

        public class When_it_was_just_below_the_nearest_interval
        {
            static double rounded;

            Because of = () => rounded = 0.59.RoundToNearest(0.3);

            It should_round_up = () => rounded.ShouldBeCloseTo(0.6, 000.1);
        }

        public class When_it_was_exactly_on_an_interval
        {
            static double rounded;

            Because of = () => rounded = 0.6.RoundToNearest(0.3);

            It should_not_round = () => rounded.ShouldBeCloseTo(0.6, 000.1);
        }

        public class When_it_was_exactly_on_an_interval_and_negative
        {
            static double rounded;

            Because of = () => rounded = -0.6.RoundToNearest(0.3);

            It should_not_round = () => rounded.ShouldBeCloseTo(-0.6, 000.1);
        }

        public class When_it_was_just_above_the_nearest_interval_and_negative
        {
            static double rounded;

            Because of = () => rounded = -0.61.RoundToNearest(0.3);

            It should_round_up = () => rounded.ShouldBeCloseTo(-0.6, 000.1);
        }

        public class When_it_was_just_below_the_nearest_interval_and_negative
        {
            static double rounded;

            Because of = () => rounded = -0.59.RoundToNearest(0.3);

            It should_round_down = () => rounded.ShouldBeCloseTo(-0.6, 000.1);
        }

        public class When_it_was_zero
        {
            static double rounded;

            Because of = () => rounded = ((double)0).RoundToNearest(0.3);

            It should_not_round = () => rounded.ShouldBeCloseTo(0, 000.1);
        }
    }
}