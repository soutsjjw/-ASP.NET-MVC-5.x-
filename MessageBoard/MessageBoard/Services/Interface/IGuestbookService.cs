﻿using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services.Interface
{
    public interface IGuestbookService
    {
        List<Guestbook> GetDataList(string search = "");

        void InsertGuestbook(Guestbook newData);

        Guestbook GetDataById(string Id);

        void UpdateGuestbook(Guestbook updateData);

        void ReplyGuestbook(Guestbook replyData);

        bool CheckUpdate(string id);

        void DeleteGuestbook(string Id);
    }
}
