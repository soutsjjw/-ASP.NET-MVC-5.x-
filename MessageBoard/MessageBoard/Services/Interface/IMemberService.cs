using MessageBoard.Models;

namespace MessageBoard.Services.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 註冊新會員
        /// </summary>
        /// <param name="newMember"></param>
        public void Register(Member newMember);

        /// <summary>
        /// Hash密碼用
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string HashPassword(string Password);

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
        public bool AccountCheck(string Account);

        /// <summary>
        /// 信箱驗證碼驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="AuthCode"></param>
        /// <returns></returns>
        public bool EmailValidate(string Account, string AuthCode, out string message);

        /// <summary>
        /// 登入帳號確認，並回傳驗證後訊息
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string LoginCheck(string Account, string Password);

        /// <summary>
        /// 進行密碼確認
        /// </summary>
        /// <param name="CheckMember"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool PasswordCheck(Member CheckMember, string Password);

        /// <summary>
        /// 取得會員的權限角色資料
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public string GetRole(string Account);
    }
}
