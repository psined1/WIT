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
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int CurrentPageNumber { get; set; }

        public string Filter { get; set; }

        // from LItemType entity:
        public int Id { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }

        public GridInfo()
        {
            TotalPages = 0;
            TotalPages = 0;
            PageSize = 0;
            CurrentPageNumber = 0;
            SortDirection = "ASC";

            Filter = "";

            Id = 0;
        }
    }

    public static class GridPagingExtension
    {
        public static IQueryable<T> Paged<T>(this IQueryable<T> q, GridInfo gridInfo)
        {
            if (!string.IsNullOrWhiteSpace(gridInfo.SortExpression))
                q = q.OrderBy(string.Format("{0} {1}", gridInfo.SortExpression, gridInfo.SortDirection));

            if (gridInfo.PageSize > 0)
            {
                int currentPage = gridInfo.CurrentPageNumber;

                if (currentPage < 1)
                    currentPage = 1;

                q = q
                    .Skip((currentPage - 1) * gridInfo.PageSize)
                    .Take(gridInfo.PageSize)
                ;
            }

            return q;
        }
    }
}
