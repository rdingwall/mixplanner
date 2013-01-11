using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace System.Data.Entity.Design.PluralizationServices
// ReSharper restore CheckNamespace
{
    public static class PluralizationServiceExtensions
    {
         public static string Pluralize<T>(
             this PluralizationService pluralizer, 
             IEnumerable<T> items, 
             string word)
         {
             if (word == null) throw new ArgumentNullException("word");

             if (items.Count() == 1)
                 return word;

             return pluralizer.Pluralize(word);
         }
    }
}