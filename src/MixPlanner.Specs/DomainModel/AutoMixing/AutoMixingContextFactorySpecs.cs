using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    [Subject(typeof(AutoMixingContextFactory))]
    public class AutoMixingContextFactorySpecs
    {
         public class When_all_the_tracks_in_the_mix_were_selected
         {
             Establish context = () =>
                                     {
                                         contextFactory = new AutoMixingContextFactory();
                                         mix = TestMixes.GetRandomMix();
                                     };

             Because of = () => mixingContext = contextFactory.CreateContext(mix, mix.Items);

             It should_include_all_the_tracks_from_the_mix =
                 () => mixingContext.TracksToMix.Should().Have.SameSequenceAs(mix.Items);

             It should_not_set_any_preceeding_track =
                 () => mixingContext.PreceedingTrack.ShouldBeNull();

             It should_not_set_any_following_track =
                 () => mixingContext.FollowingTrack.ShouldBeNull();

             static IAutoMixingContextFactory contextFactory;
             static AutoMixingContext<MixItem> mixingContext;
             static IMix mix;
         }

         public class When_a_subset_from_the_middle_of_the_mix_was_selected
         {
             Establish context = () =>
             {
                 contextFactory = new AutoMixingContextFactory();
                 mix = TestMixes.GetRandomMix(10);
                 itemsToAdd = mix.Items.Skip(2).Take(6);
             };

             Because of = () => mixingContext = contextFactory.CreateContext(mix, itemsToAdd);

             It should_add_the_expected_tracks =
                 () => mixingContext.TracksToMix.Should().Have.SameSequenceAs(itemsToAdd);

             It should_set_the_preceeding_track =
                 () => mixingContext.PreceedingTrack.ShouldEqual(mix[1]);

             It should_set_the_following_track =
                 () => mixingContext.FollowingTrack.ShouldEqual(mix[8]);

             static IAutoMixingContextFactory contextFactory;
             static AutoMixingContext<MixItem> mixingContext;
             static IMix mix;
             static IEnumerable<MixItem> itemsToAdd;
         }

         public class When_a_subset_from_the_start_of_the_mix_was_selected
         {
             Establish context = () =>
             {
                 contextFactory = new AutoMixingContextFactory();
                 mix = TestMixes.GetRandomMix(10);
                 itemsToAdd = mix.Items.Take(6);
             };

             Because of = () => mixingContext = contextFactory.CreateContext(mix, itemsToAdd);

             It should_add_the_expected_tracks =
                 () => mixingContext.TracksToMix.Should().Have.SameSequenceAs(itemsToAdd);

             It should_not_set_any_preceeding_track =
                 () => mixingContext.PreceedingTrack.ShouldBeNull();

             It should_set_the_following_track =
                 () => mixingContext.FollowingTrack.ShouldEqual(mix[6]);

             static IAutoMixingContextFactory contextFactory;
             static AutoMixingContext<MixItem> mixingContext;
             static IMix mix;
             static IEnumerable<MixItem> itemsToAdd;
         }

         public class When_a_subset_from_the_end_of_the_mix_was_selected
         {
             Establish context = () =>
             {
                 contextFactory = new AutoMixingContextFactory();
                 mix = TestMixes.GetRandomMix(10);
                 itemsToAdd = mix.Items.Skip(4);
             };

             Because of = () => mixingContext = contextFactory.CreateContext(mix, itemsToAdd);

             It should_add_the_expected_tracks =
                 () => mixingContext.TracksToMix.Should().Have.SameSequenceAs(itemsToAdd);

             It should_set_the_preceeding_track =
                 () => mixingContext.PreceedingTrack.ShouldEqual(mix[3]);

             It should_not_set_any_following_track =
                 () => mixingContext.FollowingTrack.ShouldBeNull();

             static IAutoMixingContextFactory contextFactory;
             static AutoMixingContext<MixItem> mixingContext;
             static IMix mix;
             static IEnumerable<MixItem> itemsToAdd;
         }
    }
}