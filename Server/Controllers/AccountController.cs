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
    public class AccountController : Controller
    {
        readonly bbsdbContext Context;
        readonly AccountServices accountServices;
        public AccountController(bbsdbContext context)
        {
            Context = context;
            accountServices = new AccountServices(context);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(UserModel register)
        {
            try
            {
                return Ok(accountServices.Register(register));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(UserModel login)
        {
            try
            {
                return Ok(accountServices.Login(login));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "admin")]
        public IActionResult GetAllUsers()
        {
            try
            {
                List<Users> users = accountServices.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 通过ID获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetSelfInfo()
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(accountServices.GetSelfInfo(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 通过用户名获取
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "admin")]
        public IActionResult GetUserByName(string username)
        {
            try
            {
                Users users = accountServices.GetUserByName(username);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditUserName(EditUserModel editUserModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(accountServices.EditUserName(editUserModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditUserPassword(EditUserModel editUserModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userId = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                return Ok(accountServices.EditUserPassword(editUserModel, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Policy = "admin")]
        public IActionResult EditUserByAdmin(UserManageModel user)
        {
            try
            {
                return Ok(accountServices.EditUserByAdmin(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Policy = "admin")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                return Ok(accountServices.DeleteUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetUserCount()
        {
            try
            {
                return Ok(accountServices.GetUserCount());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
