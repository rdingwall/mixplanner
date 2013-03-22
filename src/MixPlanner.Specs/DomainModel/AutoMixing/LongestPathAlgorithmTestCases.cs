using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    public static class LongestPathAlgorithmTestCases
    {
        static LongestPathAlgorithmTestCases()
        {
            MixWithMultiplePaths =
                new LongestPathAlgorithmTestCase(new[]
                                                     {
                                                         HarmonicKey.Key1A,
                                                         HarmonicKey.Key2A,
                                                         HarmonicKey.Key3A,
                                                         HarmonicKey.Key4A,
                                                         HarmonicKey.Key6A,
                                                         HarmonicKey.Key7A,
                                                         HarmonicKey.Key8A,
                                                         HarmonicKey.Key11A,
                                                     })
                    {
                        ExpectedPaths =
                            {
                                {
                                    HarmonicKey.Key11A, new[]
                                                            {
                                                                HarmonicKey.Key11A,
                                                                HarmonicKey.Key1A,
                                                                HarmonicKey.Key2A,
                                                                HarmonicKey.Key3A,
                                                                HarmonicKey.Key4A,
                                                                HarmonicKey.Key6A,
                                                                HarmonicKey.Key7A,
                                                                HarmonicKey.Key8A
                                                            }
                                },
                                {
                                    HarmonicKey.Key6A, new[]
                                                           {
                                                               HarmonicKey.Key6A,
                                                               HarmonicKey.Key7A,
                                                               HarmonicKey.Key2A,
                                                               HarmonicKey.Key3A,
                                                               HarmonicKey.Key4A,
                                                               HarmonicKey.Key11A,
                                                               HarmonicKey.Key1A,
                                                               HarmonicKey.Key8A
                                                           }
                                },
                                {
                                    HarmonicKey.Key1A, new[]
                                                           {
                                                               HarmonicKey.Key1A,
                                                               HarmonicKey.Key2A,
                                                               HarmonicKey.Key3A,
                                                               HarmonicKey.Key4A,
                                                               HarmonicKey.Key11A,
                                                               HarmonicKey.Key6A,
                                                               HarmonicKey.Key7A,
                                                               HarmonicKey.Key8A
                                                           }
                                }
                            }
                    };

            MixWithNoPaths =
                new LongestPathAlgorithmTestCase(new[]
                                                     {
                                                         HarmonicKey.Key1A,
                                                         HarmonicKey.Key2B,
                                                         HarmonicKey.Key3A,
                                                         HarmonicKey.Key4B,
                                                         HarmonicKey.Key6A,
                                                         HarmonicKey.Key7B,
                                                         HarmonicKey.Key8A,
                                                         HarmonicKey.Key11B,
                                                     });

            MixWithSingleEdge =
                new LongestPathAlgorithmTestCase(new[]
                                                     {
                                                         HarmonicKey.Key1A,
                                                         HarmonicKey.Key2A
                                                     })
                    {
                        ExpectedPaths =
                            {
                                {
                                    HarmonicKey.Key1A, new[] {HarmonicKey.Key1A, HarmonicKey.Key2A}
                                }
                            }
                    };
        }

        public static LongestPathAlgorithmTestCase MixWithMultiplePaths { get; private set; }
        public static LongestPathAlgorithmTestCase MixWithNoPaths { get; private set; }
        public static LongestPathAlgorithmTestCase MixWithSingleEdge { get; private set; }
    }
}