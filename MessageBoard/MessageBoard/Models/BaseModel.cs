using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 編號
        /// </summary>
        [DisplayName("編號")]
        [MaxLength(25)]
        public string Id { get; set; }

        public string CreatorId { get; set; }
        public UserData Creator { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        [DisplayName("新增時間")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string UpdaterId { get; set; }
        public UserData Updater { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [DisplayName("新增時間")]
        public DateTime? UpdateTime { get; set; }
    }
}
