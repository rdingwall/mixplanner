using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace MixPlanner.Specs
{
    public static class AssemblyExtensions
    {
         public static Stream GetManifestResourceStreamWithNameEnding(
             this Assembly assembly, string str)
         {
             if (str == null) throw new ArgumentNullException("str");

             var name = assembly
                 .GetManifestResourceNames()
                 .First(s => s.EndsWith(str, StringComparison.CurrentCultureIgnoreCase));

             return assembly.GetManifestResourceStream(name);
         }
    }
}