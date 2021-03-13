using MessageBoard.Repositories.Interface;

namespace MessageBoard.Services
{
    public class BaseService
    {
        protected IUnitOfWork _unitOfWork { get; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
    }
}
