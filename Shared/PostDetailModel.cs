using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class PostDetailModel
    {
        [Key]
        public int Id { get; set; }
        public int PostTypeId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public int Replys { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
        public DateTime? EditTime { get; set; }
        public int Likes { get; set; }
    }
}
