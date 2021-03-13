using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services.Interface
{
    public interface IGuestbookService
    {
        public List<Guestbook> GetDataList();

        public void InsertGuestbook(ViewModels.Guestbooks.Create newData);
    }
}
