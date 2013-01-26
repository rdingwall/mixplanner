using System;
using System.Linq;

namespace MixPlanner.Loader
{
    public static class StringUtils
    {
        public static string Coalesce(params string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            return args.FirstOrDefault(s => !String.IsNullOrWhiteSpace(s)) ?? "";
        }
    }
}