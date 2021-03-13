using MessageBoard.Data;
using MessageBoard.Models;
using MessageBoard.Repositories.Interface;

namespace MessageBoard.Repositories
{
    public class GuestbookRepository : BaseRepository<Guestbook>, IGuestbookRepository
    {
        public GuestbookRepository(MessageBoardContext messageBoardContext) :
            base(messageBoardContext)
        {
        }
    }
}
