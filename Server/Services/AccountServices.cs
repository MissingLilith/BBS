using Microsoft.IdentityModel.Tokens;
using SchoolBBS.Entity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolBBS.Shared;
using Microsoft.EntityFrameworkCore;

namespace SchoolBBS.Server.Services
{
    public class AccountServices
    {
        readonly bbsdbContext Context;
        public AccountServices(bbsdbContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Register(UserModel userModel)
        {
            bool isNameHave = Context.Users.ToList().Exists(x => x.UserName == userModel.UserName);
            if (isNameHave)
            {
                throw new Exception("用户名已被注册");
            }
            Users user = new()
            {
                UserName = userModel.UserName,
                UserPassword = userModel.UserPassword,
                CreateTime = DateTime.Now
            };
            Context.Users.Add(user);
            Context.SaveChanges();
            return true;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Login(UserModel loginModel)
        {
            Users result = Context.Users.FirstOrDefault(x => x.UserName == loginModel.UserName && x.UserPassword == loginModel.UserPassword);
            if (result != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(60*24*7)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Sid, result.UserId.ToString()),
                    new Claim("IsAdmin",result.isAdmin.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.securityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: JwtConfig.issuer,
                    audience: JwtConfig.audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60 * 24 * 7),
                    signingCredentials: creds);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
            else
            {
                throw new Exception("账号或密码错误");
            }
        }
        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        public List<Users> GetAllUsers()
        {
            return Context.Users.ToList();
        }
        /// <summary>
        /// 通过用户名获取
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Users GetUserByName(string username)
        {
            Users users = Context.Users.FirstOrDefault(x => x.UserName == username);
            if (users != null)
            {
                return users;
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }
        /// <summary>
        /// 自动获取登录id查询用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Users GetSelfInfo(int userID)
        {
            Users users = Context.Users.FirstOrDefault(x => x.UserId == userID);
            if (users != null)
            {
                return users;
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }
        /// <summary>
        /// 用户名修改
        /// </summary>
        /// <param name="editUserModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool EditUserName(EditUserModel editUserModel, int userId)
        {
            bool isNameHave = Context.Users.ToList().Exists(x => x.UserName == editUserModel.UserName && x.UserId != userId);
            if (isNameHave)
            {
                throw new Exception("用户名已存在");
            }
            Users user = Context.Users.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                user.UserName = editUserModel.UserName;
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="editUserModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool EditUserPassword(EditUserModel editUserModel, int userId)
        {
            Users users = Context.Users.FirstOrDefault(x => x.UserId == userId);
            if (users != null)
            {
                users.UserPassword = editUserModel.UserPassword;
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }
        /// <summary>
        /// 管理员修改用户
        /// </summary>
        /// <param name="userManageModel"></param>
        /// <returns></returns>
        public int EditUserByAdmin(UserManageModel userManageModel)
        {

            if (userManageModel.UserId == 0)
            {
                Users newUser = new Users();
                newUser.UserName = userManageModel.UserName;
                newUser.UserPassword = userManageModel.UserPassword;
                newUser.isAdmin = userManageModel.IsAdmin;
                newUser.CreateTime = DateTime.Now;
                Context.Users.Add(newUser);
                Context.SaveChanges();
                return newUser.UserId;
            }
            else
            {
                Users user = Context.Users.FirstOrDefault(x => x.UserId == userManageModel.UserId);
                user.UserName = userManageModel.UserName;
                user.UserPassword = userManageModel.UserPassword;
                user.isAdmin = userManageModel.IsAdmin;
                Context.SaveChanges();
                return user.UserId;
            }
        }
        /// <summary>
        /// 用户注销删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteUser(int userId)
        {
            bool IsUserHave = Context.Users.ToList().Exists(x => x.UserId == userId);
            if (IsUserHave)
            {
                Context.Users.Remove(Context.Users.FirstOrDefault(x => x.UserId == userId));
                Context.Posts.Where(x => x.CreateUserId == userId).ExecuteDelete();
                Context.PostReply.Where(x => x.CreateUserId==userId).ExecuteDelete();
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("用户不存在");
            }
        }
        public int GetUserCount()
        {
            return Context.Users.Count();
        }
    }
}
