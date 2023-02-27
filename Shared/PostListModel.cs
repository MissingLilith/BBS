using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class PostListModel
    {
        [Key]
        [DisplayName("帖子ID")]
        public int Id { get; set; }
        [DisplayName("帖子标题")]
        public string PostTitle { get; set; }
        [DisplayName("发布时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("回复量")]
        public int Replys { get; set; }
        [DisplayName("点赞量")]
        public int Likes { get; set; }

    }
}
