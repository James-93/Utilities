using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Mapping
{
    public static class MappingChildren
    {
        public static void MapChildren<T, N>(this IEnumerable<T> Parents, IEnumerable<N> Children, string MatchingField, string ChildListAtributeName)
        {
            if (Parents.Count() > 0 && Children.Count() > 0)
            {
                if (Parents.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null) && Children.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null))
                {
                    foreach (var parent in Parents)
                    {
                        var parentValue = parent.GetType().GetProperty(MatchingField).GetValue(parent, null);
                        var parentChildren = Children.Where(x => x.GetType().GetProperty(MatchingField).GetValue(x, null).ToString() == parentValue.ToString()).Select(x => (N)x);
                        parent.GetType().GetProperty(ChildListAtributeName).SetValue(parent, parentChildren);
                    }
                }
                else
                    throw new Exception($"Missing Field '{MatchingField}' does not exist in all list elements");
            }
        }
    }
}
