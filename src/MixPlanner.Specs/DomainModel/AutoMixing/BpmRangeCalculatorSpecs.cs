using System;
using Machine.Specifications;
using MixPlanner.DomainModel.AutoMixing;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    [Subject(typeof(BpmRangeCalculator))]
    public class BpmRangeCalculatorSpecs
    {
         public class When_calculating_the_number_of_degrees_away_from_the_mean
         {
             It should_correctly_calculate_112_bpm =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(112)).ShouldEqual(-2);

             It should_correctly_calculate_120_bpm =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(120)).ShouldEqual(-1);

             It should_correctly_calculate_128_bpm =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(128)).ShouldEqual(0);

             It should_correctly_calculate_137_bpm =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(137)).ShouldEqual(1);

             It should_correctly_calculate_144_bpm =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(144)).ShouldEqual(2);

             It should_set_aside_a_special_bucket_for_unknown_bpms =
                 () => ((int)BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(double.NaN)).ShouldEqual(Int32.MinValue);
         }
    }
}