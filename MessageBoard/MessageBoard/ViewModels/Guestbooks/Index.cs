using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.ViewModels.Guestbooks
{
    public class Index
    {
        public List<Guestbook> DataList { get; set; }

        public ViewModels.Guestbooks.Create Create { get; set; }

        [DisplayName("搜尋")]
        public string Search { get; set; }
    }
}
