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
    public class ReplyController : ControllerBase
    {
        readonly bbsdbContext Context;
        readonly ReplyServices replyServices;
        public ReplyController(bbsdbContext context)
        {
            Context = context;
            replyServices = new ReplyServices(context);
        }
        /// <summary>
        /// 获取全部回复
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllReplys()
        {
            try
            {
                List<PostReply> replies = replyServices.GetAllReplys();
                return Ok(replies);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetSelfReplys()
        {
            var auth = HttpContext.AuthenticateAsync();
            int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
            try
            {
                return Ok(replyServices.GetSelfReplys(userId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 获取帖子回复
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetReplysByPost(int postId)
        {
            try
            {
                return Ok(replyServices.GetReplysByPost(postId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 回复点赞
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult ReplyLiked(int replyId)
        {
            try
            {
                return Ok(replyServices.ReplyLiked(replyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="replyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult PostReply(ReplyModel replyModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(replyServices.PostReply(replyModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteReply(int replyId)
        {
            try
            {
                return Ok(replyServices.DeleteReply(replyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetReplyCount()
        {
            try
            {
                return Ok(replyServices.GetReplyCount());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
