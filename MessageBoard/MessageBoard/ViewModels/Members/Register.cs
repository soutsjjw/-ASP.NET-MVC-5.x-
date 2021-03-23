using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.ViewModels.Members
{
    public class Register
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "帳號長度須介於6~30字元")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "帳號長度最多20字元")]
        [Required(ErrorMessage = "請輸入姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 電子信箱
        /// </summary>
        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入Email")]
        [StringLength(200, ErrorMessage = "Email長度最多200字元")]
        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        public string Email { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        [DataType(DataType.Password)]
        public string PasswordCheck { get; set; }
    }
}
