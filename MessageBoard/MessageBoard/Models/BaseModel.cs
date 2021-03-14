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
    }
}
