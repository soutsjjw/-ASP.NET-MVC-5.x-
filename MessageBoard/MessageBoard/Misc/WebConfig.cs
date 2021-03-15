using MessageBoard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Misc
{
    public class WebConfig
    {
        public WebConfig()
        {
            Pagination = new PaginationContent();
        }

        public PaginationContent Pagination { get; set; }

        public class PaginationContent
        {
            public int PageSize { get; set; }
            public string PageCountAndCurrentLocationFormat { get; set; }
        }
    }
}
