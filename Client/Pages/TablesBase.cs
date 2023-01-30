using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml;

namespace SchoolBBS.Client.Pages
{
    public class TablesBase
    {

    }
    [Table("Posts")]
    public class BindItem
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("主键")]
        [AutoGenerateColumn(Ignore = true)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("标题")]
        [Required(ErrorMessage = "标题不能为空")]
        [AutoGenerateColumn(Order = 10)]
        public string? PostTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("日期")]
        [AutoGenerateColumn(Order = 1, FormatString = "yyyy-MM-dd")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("数量")]
        [AutoGenerateColumn(Order = 40)]
        public int Likes { get; set; }

    }
    [Table("Users")]
    public class BindUserItem
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("用户ID")]
        [AutoGenerateColumn(Order =1)]
        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("用户名")]
        [AutoGenerateColumn(Order =10)]
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("密码")]
        [AutoGenerateColumn(Order =20)]
        public string UserPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("是否为管理员")]
        [AutoGenerateColumn(Order =30)]
        public bool IsAdmin { get; set; }

    }
    /// <summary>
    /// BindItemContext 上下文操作类
    /// </summary>
    public class BindItemDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public BindItemDbContext(DbContextOptions<BindItemDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<BindItem>? BindItems { get; set; }
    }
}
