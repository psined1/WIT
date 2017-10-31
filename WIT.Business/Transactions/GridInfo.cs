using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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

        public GridInfo()
        {
            TotalPages = 0;
            TotalPages = 0;
            PageSize = 0;
            CurrentPageNumber = 0;
        }
    }
}
