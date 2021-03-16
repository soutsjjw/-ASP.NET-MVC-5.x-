using MessageBoard.Data;
using MessageBoard.Models;
using MessageBoard.Repositories.Interface;

namespace MessageBoard.Repositories
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(MessageBoardContext messageBoardContext) :
            base(messageBoardContext)
        {
        }
    }
}
