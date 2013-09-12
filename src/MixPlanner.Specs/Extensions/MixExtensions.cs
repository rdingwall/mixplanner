using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Extensions
{
    public static class MixExtensions
    {
        public static void Dump(this IMix mix, string name = null)
        {
            Console.WriteLine("-------------------------------------------------------");

            if (name != null)
            {
                Console.WriteLine(name);
                Console.WriteLine("-------------------------------------------------------");
            }

            for (int i = 0; i < mix.Count; i++)
            {
                IMixItem item = mix[i];

                Transition transition = item.Transition;

                string strategy = transition.Strategy == null
                                   ? "<null strategy>"
                                   : transition.Strategy.Description;

                Console.WriteLine(">>> {0} --> {1} using {2}", transition.FromKey, transition.ToKey, strategy);
                Console.WriteLine("{0}. {1}/{2:0.#}bpm {3}", i, item.ActualKey, item.ActualBpm, item.Track);
            }

            Console.WriteLine("{0} tracks total.", mix.Count);
            Console.WriteLine("-------------------------------------------------------");
        }
    }
}