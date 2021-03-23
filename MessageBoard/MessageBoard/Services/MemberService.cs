using MessageBoard.Models;
using MessageBoard.Repositories.Interface;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public class MemberService : BaseService, IMemberService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<MemberService> logger) : base(unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser newMember, string password)
        {
            if (AccountCheckAsync(newMember.UserName).Result)
            {
                var result = await _userManager.CreateAsync(newMember, password);

                return result;
            }
            else
            {
                _logger.LogWarning("嘗試註冊的帳號已有註冊資料", new { newMember.UserName });
                throw new Exception("註冊帳號發生錯誤！");
            }
        }

        public ApplicationUser GetDataByAccount(string userName)
        {
            var result = _userManager.FindByNameAsync(userName);
            result.Wait();

            return result.Result;
        }

        public ApplicationUser GetDataById(string Id)
        {
            var result = _userManager.FindByIdAsync(Id);
            result.Wait();

            return result.Result;
        }

        public async Task<bool> AccountCheckAsync(string userName)
        {
            var applicationuser = await _userManager.FindByNameAsync(userName);

            return applicationuser == null;
        }

        public bool EmailValidate(string account, string authCode, out string message)
        {
            var blPass = false;
            var validateMember = _userManager.FindByNameAsync(account);
            validateMember.Wait();

            if (validateMember.Result == null)
            {
                message = "傳送資料錯誤，請重新確認或再註冊";
            }
            else
            {
                authCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(authCode));
                var result = _userManager.ConfirmEmailAsync(validateMember.Result, authCode);
                result.Wait();

                if (result.Result.Succeeded)
                {
                    message = "帳號信箱驗證成功，現在可以輸入了";
                    blPass = true;
                }
                else
                {
                    message = "驗證碼錯誤，請重新確認或再註冊";
                }
            }

            return blPass;
        }

        public async Task<string> LoginCheckAsync(string account, string password)
        {
            var applicationUser = await _userManager.FindByNameAsync(account);

            if (applicationUser == null)
            {
                return "無此會員帳號或密碼錯誤，若您要加入會員請按「去註冊」。";
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(applicationUser, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return string.Empty;
                }

                if (result.IsLockedOut)
                {
                    return "此帳號尚未經過Email驗證，請去收信";
                }
                else
                {
                    return "無此會員帳號或密碼錯誤。";
                }
            }
        }

        public string GetRole(string Account)
        {
            string role = "User";
            var loginMember = GetDataByAccount(Account);
            if (loginMember.IsAdmin)
            {
                role += ",Admin";
            }
            return role;
        }

        public bool ChangePassword(ApplicationUser applicationUser, string password, string newPassword, out string message)
        {
            bool blPass = false;
            var resultWait = _userManager.ChangePasswordAsync(applicationUser, password, newPassword);
            resultWait.Wait();

            if (resultWait.Result.Succeeded)
            {
                blPass = true;
                message = "密碼修改成功";
            }
            else
            {
                message = resultWait.Result.Errors.First().Description;
            }

            return blPass;
        }
    }
}
