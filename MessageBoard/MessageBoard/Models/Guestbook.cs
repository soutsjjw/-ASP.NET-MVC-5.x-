using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBoard.Models
{
    /// <summary>
    /// 用於儲存留言板資料內容
    /// </summary>
    public class Guestbook : BaseModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(30, ErrorMessage = "帳號不可超過30字元")]
        [Column(TypeName = "varchar(30)")]
        public string Account { get; set; }
        /// <summary>
        /// 留言內容
        /// </summary>
        [DisplayName("留言內容")]
        [Required(ErrorMessage = "請輸入留言內容")]
        [StringLength(100, ErrorMessage = "留言內容不可超過100字元")]
        public string Content { get; set; }
        /// <summary>
        /// 新增時間
        /// </summary>
        [DisplayName("新增時間")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 回覆內容
        /// </summary>
        [DisplayName("回覆內容")]
        [StringLength(100, ErrorMessage = "回覆內容不可超過100字元")]
        public string Reply { get; set; }
        /// <summary>
        /// 回覆時間
        /// </summary>
        [DisplayName("回覆時間")]
        public DateTime? ReplyTime { get; set; }
    }
}
