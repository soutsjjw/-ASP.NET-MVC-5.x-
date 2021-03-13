using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Repositories.Interface
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 儲存所有異動
        /// </summary>
        void Commit();
    }
}
