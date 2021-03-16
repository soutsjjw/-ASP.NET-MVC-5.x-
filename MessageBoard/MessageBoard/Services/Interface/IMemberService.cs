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
    }
}
