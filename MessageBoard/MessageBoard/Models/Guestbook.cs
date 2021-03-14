using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    /// <summary>
    /// 用於儲存留言板資料內容
    /// </summary>
    public class Guestbook : BaseModel
    {
        /// <summary>
        /// 名字
        /// </summary>
        [DisplayName("名字")]
        [Required(ErrorMessage = "請輸入名字")]
        [StringLength(20, ErrorMessage = "名字不可超過20字元")]
        public string Name { get; set; }
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
