using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services.Interface
{
    public interface IMailService
    {
        /// <summary>
        /// 產生驗證碼
        /// </summary>
        /// <returns></returns>
        public string GetValidateCode();

        /// <summary>
        /// 將使用者資料填入驗證信範本中
        /// </summary>
        /// <param name="TempString"></param>
        /// <param name="UserName"></param>
        /// <param name="ValidateUrl"></param>
        /// <returns></returns>
        public string GetRegiesterMailBody(string TempString, string UserName, string ValidateUrl);

        /// <summary>
        /// 寄驗證信
        /// </summary>
        /// <param name="MailBody"></param>
        /// <param name="ToEmail"></param>
        public Task SendRegisterMailAsync(string MailBody, string ToEmail);
    }
}
