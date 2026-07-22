using System.Collections.Generic;
using System.Linq;

namespace demo_project.app
{
    public static class ListExtension
    {
        public static string ToStringWithJoin(this List<bool?> list)
        {
            if (list is null || list?.Count == 0) return "";
            list = list.Where(x => x != null).ToList();
            if (list is null || list?.Count == 0) return "";

            return string.Join(',', list.Select(x => x.Value ? 1 : 0));
        }

        public static string ToStringWithJoin(this List<long?> list)
        {
            if (list is null || list?.Count == 0) return "";
            list = list.Where(x => x != null).ToList();
            if (list is null || list?.Count == 0) return "";

            return string.Join(',', list);
        }

        public static string ToStringWithJoin(this List<string> list)
        {
            if (list is null || list?.Count == 0) return "";
            list = list.Where(x => x != null).ToList();
            if (list is null || list?.Count == 0) return "";

            return string.Join(',', list);
        }
    }
}