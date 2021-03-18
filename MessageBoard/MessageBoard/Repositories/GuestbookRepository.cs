using MessageBoard.Data;
using MessageBoard.Models;
using MessageBoard.Repositories.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace MessageBoard.Repositories
{
    public class GuestbookRepository : BaseRepository<Guestbook>, IGuestbookRepository
    {
        public GuestbookRepository(MessageBoardContext messageBoardContext) :
            base(messageBoardContext)
        {
            
        }

        public override IEnumerable<Guestbook> Get(Expression<Func<Guestbook, bool>> filter = null, Func<IQueryable<Guestbook>, IOrderedQueryable<Guestbook>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Guestbook> query = this.GetAll();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public override IQueryable<Guestbook> GetAll()
        {
            return _dbSet.Include(x => x.Member);
        }
    }
}
