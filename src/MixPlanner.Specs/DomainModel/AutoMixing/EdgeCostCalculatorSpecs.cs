using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MoreLinq;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    [Subject(typeof(EdgeCostCalculator))]
    public class EdgeCostCalculatorSpecs
    {
         public class When_calculating_the_expected_edge_costs_of_different_transitions_between_tracks
         {
             Establish context =
                 () =>
                     {
                         expectedOrder = new[]
                                          {
                                              new Case("A", HarmonicKey.Key8A, HarmonicKey.Key8A), // perfect
                                              new Case("B", HarmonicKey.Key8A, HarmonicKey.Key10A), // perfect
                                              new Case("C", HarmonicKey.Key8A, HarmonicKey.Key9A), // perfect
                                              new Case("D", HarmonicKey.Key8A, 131, HarmonicKey.Key8A, 128), // +3%
                                              new Case("E", HarmonicKey.Key8A, 128, HarmonicKey.Key8A, 133), // -3%
                                              new Case("F", HarmonicKey.Key8A, 131, HarmonicKey.Key8A, 124), // +6%
                                              new Case("G", HarmonicKey.Key8A, 124, HarmonicKey.Key8A, 131), // -6%
                                              new Case("H", HarmonicKey.Key8A, HarmonicKey.Key7A), // non-preferred
                                              new Case("I", HarmonicKey.Key9A, 131, HarmonicKey.Key1A, 122), // +6% non-preferred
                                              new Case("J", HarmonicKey.Key9A, 122, HarmonicKey.Key1A, 133), // -6% non-preferred
                                              new Case("K", HarmonicKey.Key1A, HarmonicKey.Key5B), // key trainwreck 
                                              new Case("L", HarmonicKey.Key8A, 999, HarmonicKey.Key8A, 1) // bpm trainwreck
                                          };

                         randomOrder = expectedOrder.Shuffle().ToList();

                         container = new WindsorContainer();
                         container.Install(new IocRegistrations());
                         container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                         calculator = container.Resolve<IEdgeCostCalculator>();
                     };

             Because of = () => sorted = randomOrder
                                             .Pipe(c => c.Cost = calculator.CalculateCost(c.PreviousSpeed, c.NextSpeed))
                                             .OrderBy(c => c.Name) // keeps equal cost cases in the same order
                                             .OrderBy(c => c.Cost);
                 

             It should_sort_them_in_the_correct_order =
                 () => sorted.Should().Have.SameSequenceAs(expectedOrder);

             Cleanup after = () => container.Dispose();

             static IWindsorContainer container;
             static IEdgeCostCalculator calculator;
             static List<Case> randomOrder;
             static IOrderedEnumerable<Case> sorted;
             static Case[] expectedOrder;

            class Case
             {
                 public string Name { get; private set; }
                 public EdgeCost Cost { get; set; }
                 public PlaybackSpeed PreviousSpeed { get; private set; }
                 public PlaybackSpeed NextSpeed { get; private set; }

                 public Case(string name, HarmonicKey previousKey, 
                     HarmonicKey nextKey) : this(name, previousKey, 128, nextKey, 128)
                 {
                 }

                 public Case(string name, HarmonicKey previousKey, double previousBpm,
                     HarmonicKey nextKey, double nextBpm)
                 {
                     Name = name;
                     PreviousSpeed = new PlaybackSpeed(previousKey, previousBpm);
                     NextSpeed = new PlaybackSpeed(nextKey, nextBpm);
                 }

                 public override string ToString()
                 {
                     return String.Format("{0}: {1}", Name, Cost);
                 }
             }
         }
    }
}