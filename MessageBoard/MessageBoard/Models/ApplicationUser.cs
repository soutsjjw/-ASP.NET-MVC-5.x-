using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "帳號長度最多20字元")]
        [Required(ErrorMessage = "請輸入姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 是否為管理員
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
