using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MappingObjects
    {
        public static void MapChildren(this IEnumerable<object> Parents, IEnumerable<object> Children, string MatchingField, string ChildListAtributeName)
        {
            if (Parents.Count() > 0 && Children.Count() > 0)
            {
                if (Parents.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null) && Children.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null)) {
                    Parallel.ForEach(Parents, parent =>
                    {
                        var parentValue = parent.GetType().GetProperty(MatchingField).GetValue(parent, null);
                        var parentChildren = Children.Where(x => x.GetType().GetProperty(MatchingField).GetValue(x, null) == parentValue);
                        parent.GetType().GetProperty(ChildListAtributeName).SetValue(parent, parentChildren);
                    });
                }
                else
                    throw new Exception($"Missing Field '{MatchingField}' does not exist in all list elements");
            }
        }
    }
}
