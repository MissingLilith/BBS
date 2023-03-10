using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolBBS.Entity.Models;
using SchoolBBS.Server.Services;
using SchoolBBS.Shared;
using System.Security.Claims;

namespace SchoolBBS.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : Controller
    {
        readonly bbsdbContext Context;
        readonly PostServices postServices;
        public PostController(bbsdbContext context)
        {
            Context = context;
            postServices = new PostServices(context);
        }
        /// <summary>
        /// 获取全部帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            try
            {
                List<Posts> posts = postServices.GetAllPosts();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPostTypes()
        {
            try
            {
                List<PostTypes> posts = postServices.GetAllPostTypes();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 按时间排序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPostByTime()
        {
            try
            {
                List<Posts> posts=postServices.GetPostByTime();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 按热门排序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPostByLikes()
        {
            try
            {
                List<Posts> posts=postServices.GetPostByLikes();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 发帖
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddPost(PostCreateModel postModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(postServices.AddPost(postModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 添加帖子类型
        /// </summary>
        /// <param name="postTypeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "admin")]
        public IActionResult AddPostType(string postTypeName)
        {
            try
            {
                return Ok(postServices.AddPostType(postTypeName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPostType(int typeId)
        {
            try
            {
                return Ok(postServices.GetPostTypeName(typeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 查询当前用户发帖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetSelfPosts()
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(postServices.GetSelfPosts(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 按类型获取帖子
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPostsByTypeId(int typeId)
        {
            try
            {
                return Ok(postServices.GetPostsByTypeId(typeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 由Id获取帖子详情
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPostDetail(int postId)
        {
            try
            {
                return Ok(postServices.GetPostDetail(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 帖子点赞
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult PostLiked(int postId)
        {
            try
            {
                return Ok(postServices.PostLiked(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="postManageModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditPost(PostManageModel postManageModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(postServices.EditPost(postManageModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 管理员编辑
        /// </summary>
        /// <param name="postDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult EditPostDetail(PostDetailModel postDetailModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(postServices.EditPostDetail(postDetailModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                return Ok(postServices.DeletePost(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [EnableCors("any")]
        public IActionResult GetPostCount()
        {
            try
            {
                return Ok(postServices.GetPostCount());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetStatistics()
        {
            try
            {
                return Ok(postServices.GetStatistics());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [EnableCors("any")]
        public IActionResult GetPostDateNum()
        {
            try
            {
                var dateNum = Context.Posts.OrderBy(x => x.CreateTime).GroupBy(z => new
                {
                    year = z.CreateTime.Value.Year,
                    month = z.CreateTime.Value.Month,
                }, (x, y) => new
                {
                    num = y.Count(),
                    x.year,
                    x.month,
                    date = x.year + "-" + (x.month >= 10 ? x.month.ToString() : "0" + x.month)
                }).OrderBy(x => x.year).ThenBy(x => x.month);
                return Ok(dateNum);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
