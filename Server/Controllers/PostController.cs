using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
            postServices=new PostServices(context);
        }
        /// <summary>
        /// 获取全部帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "admin")]
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
        [HttpGet]
        [Authorize(Policy = "admin")]
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
                return Ok(postServices.AddPost(postModel,userId));
            }
            catch(Exception ex)
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
