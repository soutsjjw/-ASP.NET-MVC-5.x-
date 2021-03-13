using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.ViewModels.Guestbooks
{
    public class Index
    {
        public List<Guestbook> DataList { get; set; }

        public MessageBoard.ViewModels.Guestbooks.Create Create { get; set; }
    }
}
