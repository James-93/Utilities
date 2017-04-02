using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MappingObjects
    {
        public List<object> MapChildren(IEnumerable<object> Parents, string ChildListAtributeName, IEnumerable<object> Children, string MatchingField, bool DefaultParentsToEmptyList = true)
        {
            if (Parents.Count() > 0 && Children.Count() > 0)
            {
                if (Parents.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null) && Children.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null)) {
                    if (Parents.Count() > 10000)
                    {
                        Parallel.ForEach(Parents, parent =>
                        {
                            var parentValue = parent.GetType().GetProperty(MatchingField).GetValue(parent, null);
                            var parentChildren = Children.Where(x => x.GetType().GetProperty(MatchingField).GetValue(x, null) == parentValue);
                            parent.GetType().GetProperty(ChildListAtributeName).SetValue(parent, parentChildren);
                        });
                    }
                    else
                    {
                        foreach (var parent in Parents)
                        {
                            var parentValue = parent.GetType().GetProperty(MatchingField).GetValue(parent, null);
                            var parentChildren = Children.Where(x => x.GetType().GetProperty(MatchingField).GetValue(x, null) == parentValue);
                            parent.GetType().GetProperty(ChildListAtributeName).SetValue(parent, parentChildren);
                        }
                    }
                }
                else
                    throw new Exception($"Missing Field '{MatchingField}' does not exist in all list elements");
            }
            return Parents.ToList();
        }
    }
}
