using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.Mapping
{
  public static class MappingChildren
  {
    public static void MapChildren<T, N>(this IEnumerable<T> Parents, IEnumerable<N> Children, string MatchingField)
    {
      if (Parents.Count() > 0 && Children.Count() > 0)
      {
        if (Parents.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null) && Children.Select(x => x.GetType().GetProperty(MatchingField)).All(x => x != null))
        {
          string listPattern = String.Format("List..\\[{0}\\]", Children.FirstOrDefault().GetType().ToString());
          var r = new Regex(listPattern, RegexOptions.IgnoreCase);

          string ChildListAtributeName = Parents.FirstOrDefault().GetType().GetProperties().Where(x => r.Match(x.PropertyType.ToString()).Success).Select(x => x.Name).FirstOrDefault();

          if (ChildListAtributeName == null)
          {
            string ienumerablePattern = String.Format("IEnumerable..\\[{0}\\]", Children.FirstOrDefault().GetType().ToString());
            r = new Regex(ienumerablePattern, RegexOptions.IgnoreCase);

            ChildListAtributeName = Parents.FirstOrDefault().GetType().GetProperties().Where(x => r.Match(x.PropertyType.ToString()).Success).Select(x => x.Name).FirstOrDefault();
          }

          if (ChildListAtributeName != null)
          {
            Parallel.ForEach(Parents, parent =>
            {
              var parentValue = parent.GetType().GetProperty(MatchingField).GetValue(parent, null);
              var parentChildren = Children.Where(x => x.GetType().GetProperty(MatchingField).GetValue(x, null).ToString() == parentValue.ToString()).Select(x => (N)x);
              parent.GetType().GetProperty(ChildListAtributeName).SetValue(parent, parentChildren.ToList());
            });
          }
          else
            throw new Exception("Parent object does not contain List or IEnumerable of child object");
        }
        else
          throw new Exception(String.Format("Missing Field '{0}' does not exist in all list elements", MatchingField));
      }
    }
  }
}
