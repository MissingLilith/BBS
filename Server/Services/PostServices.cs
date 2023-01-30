using Microsoft.IdentityModel.Tokens;
using SchoolBBS.Entity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolBBS.Shared;

namespace SchoolBBS.Server.Services
{
    public class PostServices
    {
        readonly bbsdbContext Context;
        public PostServices(bbsdbContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 获取所有帖子
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Posts> GetAllPosts()
        {
            List<Posts> posts = Context.Posts.ToList();
            if (posts.Count > 0)
            {
                return posts;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        public List<PostTypes> GetAllPostTypes()
        {
            List<PostTypes> postTypes = Context.PostTypes.ToList();
            if(postTypes.Count > 0)
            {
                return postTypes;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        /// <summary>
        /// 发帖
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddPost(PostCreateModel postModel,int userId)
        {
            bool isTitleHave = Context.Posts.ToList().Exists(x => x.PostTitle == postModel.PostTitle);
            bool isTypeHave=Context.PostTypes.ToList().Exists(x=>x.Id== postModel.PostTypeId);
            if (isTitleHave)
            {
                throw new Exception("标题与其他帖子重复");
            }
            if (!isTypeHave)
            {
                throw new Exception("帖子类型不存在");
            }
            Posts post = new()
            {
                PostTitle = postModel.PostTitle,
                PostContent = postModel.PostContent,
                PostTypeId = postModel.PostTypeId,
                CreateTime = DateTime.Now,
                CreateUserId = userId
            };
            Context.Posts.Add(post);
            Context.SaveChanges();
            return true;
        }
        /// <summary>
        /// 添加帖子类型
        /// </summary>
        /// <param name="postTypeModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddPostType(string postTypeName)
        {
            bool isTypeHave = Context.PostTypes.ToList().Exists(x => x.PostTypeName == postTypeName);
            if (isTypeHave)
            {
                throw new Exception("该类型已存在");
            }
            PostTypes postType = new()
            {
                PostTypeName = postTypeName
            };
            Context.PostTypes.Add(postType);
            Context.SaveChanges();
            return true;
        }
        public List<Posts> GetSelfPosts(int userId) { 
        List<Posts> posts = Context.Posts.Where(x=>x.CreateUserId== userId).ToList();
            if(posts.Count>0)
            {
                return posts;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
    }
}
