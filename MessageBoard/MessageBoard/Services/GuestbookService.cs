using AutoMapper;
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

        public List<Guestbook> GetDataList(string search = "")
        {
            if (string.IsNullOrEmpty(search))
                return _guestbookRepository.GetAll().ToList();
            else
            {
                return _guestbookRepository
                    .Get(x => x.Name.Contains(search) || x.Content.Contains(search) || (!string.IsNullOrWhiteSpace(x.Reply) && x.Reply.Contains(search)))
                    .ToList();
            }
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
            var entity = _guestbookRepository.Get()
                .ToList()
                .Where(x => x.Id == Id)
                .FirstOrDefault();

            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
            }

            return entity;
        }

        public void UpdateGuestbook(Guestbook updateData)
        {
            var entity = GetDataById(updateData.Id);

            entity.Name = updateData.Name;
            entity.Content = updateData.Content;

            _guestbookRepository.Update(entity);
            _unitOfWork.Commit();
        }

        public void ReplyGuestbook(Guestbook replyData)
        {
            var entity = GetDataById(replyData.Id);

            entity.Reply = replyData.Reply;
            entity.ReplyTime = DateTime.Now;

            _guestbookRepository.Update(entity);
            _unitOfWork.Commit();
        }

        public bool CheckUpdate(string Id)
        {
            var entity = GetDataById(Id);

            return !entity.ReplyTime.HasValue;
        }

        public void DeleteGuestbook(string Id)
        {
            Guestbook entity = GetDataById(Id);

            _guestbookRepository.Delete(entity);
            _unitOfWork.Commit();
        }
    }
}
