using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MessageBoard.ViewModels.Guestbooks
{
    public class Index
    {
        public IPagedList<Guestbook> DataList { get; set; }

        public ViewModels.Guestbooks.Create Create { get; set; }

        [DisplayName("搜尋")]
        public string Search { get; set; }
    }
}
