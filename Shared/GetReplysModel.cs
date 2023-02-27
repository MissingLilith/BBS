using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class GetReplysModel
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("帖子ID")]
        public int PostId { get; set; }
        [DisplayName("回复内容")]
        public string ReplyContent { get; set; }
        [DisplayName("回复时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("用户ID")]
        public int CreateUserId { get; set; }
        [DisplayName("点赞量")]
        public int Likes { get; set; }
    }
}
