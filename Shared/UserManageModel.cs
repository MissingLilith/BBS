using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBBS.Shared
{
    public class UserManageModel
    {
        [DisplayName("用户ID")]
        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("用户名")]
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("密码")]
        [Required]
        public string UserPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("是否为管理员")]
        public bool IsAdmin { get; set; }
    }
}
