using MessageBoard.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.ViewModels.Members
{
    public class Register
    {
        public Member newMember { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        [DataType(DataType.Password)]
        public string PasswordCheck { get; set; }
    }
}
