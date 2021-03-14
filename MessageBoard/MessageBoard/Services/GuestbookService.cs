﻿using AutoMapper;
using MessageBoard.Helpers;
using MessageBoard.Models;
using MessageBoard.Repositories.Interface;
using MessageBoard.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public class GuestbookService : BaseService, IGuestbookService
    {
        private readonly IMapper _mapper;
        private readonly IGuestbookRepository _guestbookRepository;

        public GuestbookService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IGuestbookRepository guestbookRepository)
            : base(unitOfWork)
        {
            _mapper = mapper;
            _guestbookRepository = guestbookRepository;
        }

        public List<Guestbook> GetDataList()
        {
            return _guestbookRepository.Get().ToList();
        }

        public void InsertGuestbook(Guestbook newData)
        {
            newData.Id = MiscHelper.ShortGuid;
            newData.CreateTime = DateTime.Now;

            _guestbookRepository.Insert(newData);
            _unitOfWork.Commit();
        }

        public Guestbook GetDataById(string Id)
        {
            return _guestbookRepository.Get()
                .ToList()
                .Where(x => x.Id == Id)
                .FirstOrDefault();
        }

        public void UpdateGuestbook(Guestbook updateData)
        {
            var entity = GetDataById(updateData.Id);
            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
            }

            entity.Name = updateData.Name;
            entity.Content = updateData.Content;

            _guestbookRepository.Update(entity);
            _unitOfWork.Commit();
        }

        public void ReplyGuestbook(Guestbook replyData)
        {
            var entity = GetDataById(replyData.Id);
            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
            }

            entity.Reply = replyData.Reply;
            entity.ReplyTime = DateTime.Now;

            _guestbookRepository.Update(entity);
            _unitOfWork.Commit();
        }

        public bool CheckUpdate(string id)
        {
            var entity = GetDataById(id);
            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
            }

            return !entity.ReplyTime.HasValue;
        }
    }
}
