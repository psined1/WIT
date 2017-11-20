using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Linq.Dynamic;

namespace WIT.Business.Entities
{
    public class GridInfo
    {
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public int PageSize { get; set; }
        public int SortId { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int CurrentPageNumber { get; set; }

        public string Filter { get; set; }

        // from LItemType entity:
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }

        public GridInfo()
        {
            TotalPages = 0;
            TotalPages = 0;
            PageSize = 0;
            CurrentPageNumber = 0;
            SortDirection = "ASC";
            SortId = 0;

            Filter = "";

            ItemTypeId = 0;
        }
    }

    public static class GridPagingExtension
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> q, GridInfo info)
        {
            if (!string.IsNullOrWhiteSpace(info.SortExpression))
            {
                if (string.IsNullOrWhiteSpace(info.SortDirection))
                    info.SortDirection = "ASC";

                q = q.OrderBy(string.Format("{0} {1}", info.SortExpression, info.SortDirection));
            }

            if (info.PageSize > 0)
            {
                if (info.CurrentPageNumber < 1)
                    info.CurrentPageNumber = 1;

                q = q
                    .Skip((info.CurrentPageNumber - 1) * info.PageSize)
                    .Take(info.PageSize)
                ;
            }

            return q;
        }
    }
}
