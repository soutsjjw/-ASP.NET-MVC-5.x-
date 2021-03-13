﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.ViewModels.Guestbooks
{
    public class Create
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
    }
}