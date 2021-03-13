using AutoMapper;
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
            return _guestbookRepository.Query().ToList();
        }

        public void InsertGuestbook(ViewModels.Guestbooks.Create newData)
        {
            var entity = _mapper.Map<Guestbook>(newData);
            entity.CreateTime = DateTime.Now;

            _guestbookRepository.Insert(entity);
            _unitOfWork.Commit();
        }
    }
}
