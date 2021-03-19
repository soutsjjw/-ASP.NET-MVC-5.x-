using MessageBoard.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MessageBoard.Services.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 註冊新會員
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="password"></param>
        public Task<IdentityResult> RegisterAsync(Member newMember, string password);

        /// <summary>
        /// 藉由帳號取得單筆資料
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public Member GetDataByAccount(string Account);

        /// <summary>
        /// 藉由Id取得單筆資料
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public Member GetDataById(string Id);

        /// <summary>
        /// 確認要註冊帳號是否有被註冊過
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public Task<bool> AccountCheckAsync(string Account);

        /// <summary>
        /// 信箱驗證碼驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="AuthCode"></param>
        /// <returns></returns>
        // public bool EmailValidateAsync(string Account, string AuthCode, out string message);
        public bool EmailValidate(string account, string authCode, out string message);

        /// <summary>
        /// 登入帳號確認，並回傳驗證後訊息
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<string> LoginCheckAsync(string Account, string Password);

        /// <summary>
        /// 取得會員的權限角色資料
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public string GetRole(string Account);

        /// <summary>
        /// 變更會員密碼，並回傳最後訊息
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(ApplicationUser applicationUser, string password, string newPassword, out string message);
    }
}
