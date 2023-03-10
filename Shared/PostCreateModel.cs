using SchoolBBS.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class PostCreateModel
    {
        public string PostTitle { get; set; }
        public PostContent PostContent { get; set; }
        public int PostTypeId { get; set; }
    }
}
