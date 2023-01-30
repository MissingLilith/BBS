using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class SelfPostsModel
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public int PostTypeId { get; set; }
        public int Clicks { get; set; }
        public int Replys { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastReplyTime { get; set; }
        public int Likes { get; set; }
    }
}
