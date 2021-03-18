using MessageBoard.Services.Interface;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public class MailService : IMailService
    {
        private readonly Models.WebConfig _config;
        private readonly IEmailService _emailService;

        public MailService(Models.WebConfig config,
            IEmailService emailService)
        {
            _config = config;
            _emailService = emailService;
        }

        public string GetValidateCode()
        {
            string[] Code = { "A","B","C","D","E","F","G","H","I",
"J","K","L","M","N","O","P","Q","R","S","T","U",
"V","W","X","Y","Z","1","2","3","4","5","6",
"7","8","9","a","b","c","d","e","f","g","h"
,"i","j","k","l","m","n","o","p","q","s","t",
"u","v","w","x","y","z" };
            string validateCode = string.Empty;
            Random rd = new Random();

            for (int i = 0; i < 10; i++)
            {
                validateCode += Code[rd.Next(Code.Count())];
            }

            return validateCode;
        }

        public string GetRegiesterMailBody(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);

            return TempString;
        }

        public async Task SendRegisterMailAsync(string MailBody, string ToEmail)
        {
            await _emailService.SendAsync(ToEmail, "會員註冊確認信", MailBody, true);
        }
    }
}
