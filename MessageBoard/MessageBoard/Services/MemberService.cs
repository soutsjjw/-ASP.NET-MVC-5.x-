using MessageBoard.Helpers;
using MessageBoard.Models;
using MessageBoard.Repositories;
using MessageBoard.Repositories.Interface;
using MessageBoard.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public class MemberService : BaseService, IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork,
            ILogger<MemberService> logger,
            IMemberRepository memberRepository) : base(unitOfWork)
        {
            _logger = logger;
            _memberRepository = memberRepository;
        }

        public void Register(Member newMember)
        {
            if (AccountCheck(newMember.Account))
            {
                newMember.Id = MiscHelper.ShortGuid;
                newMember.Password = HashPassword(newMember.Password);
                _memberRepository.Insert(newMember);
                _unitOfWork.Commit();
            }
            else
            {
                _logger.LogWarning("嘗試註冊的帳號已有註冊資料", new { newMember.Account });
                throw new Exception("註冊帳號發生錯誤！");
            }
        }

        public string HashPassword(string Password)
        {
            // 宣告Hash時所添加的無意義亂數值
            string saltKey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            // 將剛剛宣告的字串與密碼結合
            string saltAndPassword = String.Concat(Password, saltKey);
            // 定義SHA256的HASH物件
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            // 取得密碼轉換成byte資料
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            // 取得Hash後byte資料
            byte[] HashData = sha256Hasher.ComputeHash(PasswordData);
            // 將Hash後byte資料轉換成string
            string Hashresult = Convert.ToBase64String(HashData);
            // 回傳Hash後結果
            return Hashresult;
        }

        public Member GetDataByAccount(string Account)
        {
            return _memberRepository.Get(x => x.Account.Equals(Account)).FirstOrDefault();
        }

        public bool AccountCheck(string Account)
        {
            return GetDataByAccount(Account) == null;
        }

        public bool EmailValidate(string Account, string AuthCode, out string message)
        {
            var validateMember = GetDataByAccount(Account);
            var validateStr = string.Empty;
            var blPass = false;

            if (validateMember == null)
            {
                validateStr = "傳送資料錯誤，請重新確認或再註冊";
            }
            else
            {
                if (validateMember.AuthCode.Equals(AuthCode))
                {
                    validateMember.AuthCode = string.Empty;
                    _memberRepository.Update(validateMember);
                    _unitOfWork.Commit();

                    validateStr = "帳號信箱驗證成功，現在可以輸入了";
                    blPass = true;
                }
                else
                {
                    validateStr = "驗證碼錯誤，請重新確認或再註冊";
                }
            }

            message = validateStr;

            return blPass;
        }

        public string LoginCheck(string Account, string Password)
        {
            Member loginMember = GetDataByAccount(Account);

            if (loginMember == null)
            {
                return "無此會員帳號或密碼錯誤，若您要加入會員請按「去註冊」。";
            }
            else
            {
                if (PasswordCheck(loginMember, Password))
                {
                    if (string.IsNullOrWhiteSpace(loginMember.AuthCode))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return "此帳號尚未經過Email驗證，請去收信";
                    }
                }
                else
                    return "無此會員帳號或密碼錯誤，若您要加入會員請按「註冊」。";
            }
        }

        public bool PasswordCheck(Member CheckMember, string Password)
        {
            return CheckMember.Password.Equals(HashPassword(Password));
        }

        public string GetRole(string Account)
        {
            string role = "User";
            Member loginMember = GetDataByAccount(Account);
            if (loginMember.IsAdmin)
            {
                role += ",Admin";
            }
            return role;
        }
    }
}
