using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services.Interface
{
    public interface IJwtService
    {
        /// <summary>
        /// 製作Token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public string GenerateToken(string accountId, string account, string role);
    }
}
