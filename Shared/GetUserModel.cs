using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class GetUserModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsAdmin { get; set; }
    }
}
