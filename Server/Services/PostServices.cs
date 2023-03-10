using Microsoft.IdentityModel.Tokens;
using SchoolBBS.Entity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolBBS.Shared;
using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;

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
            if (postTypes.Count > 0)
            {
                return postTypes;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        public List<Posts> GetPostByTime()
        {
            List<Posts> posts = Context.Posts.OrderByDescending(x => x.CreateTime).Take(20).ToList();
            if (posts.Count > 0)
            {
                return posts;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        public List<Posts> GetPostByLikes()
        {
            List<Posts> posts=Context.Posts.OrderByDescending(x => x.Likes).Take(20).ToList();
            if (posts.Count > 0)
            {
                return posts;
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
        public bool AddPost(PostCreateModel postModel, int userId)
        {
            bool isTitleHave = Context.Posts.ToList().Exists(x => x.PostTitle == postModel.PostTitle);
            bool isTypeHave = Context.PostTypes.ToList().Exists(x => x.Id == postModel.PostTypeId);
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
        /// <param name="postTypeName"></param>
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
        /// <summary>
        /// 获取分类名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetPostTypeName(int typeId)
        {
            bool isTypeHave = Context.PostTypes.ToList().Exists(x => x.Id == typeId);
            if (isTypeHave)
            {
                return Context.PostTypes.FirstOrDefault(x => x.Id == typeId).PostTypeName;
            }
            else
            {
                throw new Exception("该分类不存在");
            }
        }
        /// <summary>
        /// 获取个人帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Posts> GetSelfPosts(int userId)
        {
            List<Posts> posts = Context.Posts.Where(x => x.CreateUserId == userId).ToList();
            if (posts.Count > 0)
            {
                return posts;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        /// <summary>
        /// 按类型获取帖子
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Posts> GetPostsByTypeId(int typeId)
        {
            bool isTypeHave = Context.PostTypes.ToList().Exists(x => x.Id == typeId);
            if (isTypeHave)
            {
                return Context.Posts.Where(x => x.PostTypeId == typeId).ToList();
            }
            else
            {
                throw new Exception("获取数据失败");
            }
        }
        /// <summary>
        /// 由ID获取帖子详情
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Posts GetPostDetail(int postId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == postId);
            if (isPostHave)
            {
                return Context.Posts.FirstOrDefault(x => x.Id == postId);
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 点赞帖子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool PostLiked(int postId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == postId);
            if (isPostHave)
            {
                Posts post = Context.Posts.FirstOrDefault(x => x.Id == postId);
                post.Likes += 1;
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 简单修改
        /// </summary>
        /// <param name="postManageModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool EditPost(PostManageModel postManageModel, int userId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == postManageModel.Id);
            if (isPostHave)
            {
                Posts post = Context.Posts.FirstOrDefault(x => x.Id == postManageModel.Id);
                post.PostTitle = postManageModel.PostTitle;
                post.PostTypeId = postManageModel.PostTypeId;
                post.EditTime = DateTime.Now;
                post.EditUserId = userId;
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 内容修改
        /// </summary>
        /// <param name="postDetailModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool EditPostDetail(PostDetailModel postDetailModel, int userId)
        {
            Posts post = Context.Posts.FirstOrDefault(x => x.Id == postDetailModel.Id);
            post.PostTitle = postDetailModel.PostTitle;
            post.PostContent = postDetailModel.PostContent;
            post.PostTypeId = postDetailModel.PostTypeId;
            post.EditUserId = userId;
            post.EditTime = DateTime.Now;
            Context.SaveChanges();
            return true;
        }
        /// <summary>
        /// 帖子删除
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeletePost(int postId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == postId);
            if (isPostHave)
            {
                Context.Posts.Remove(Context.Posts.FirstOrDefault(x => x.Id == postId));
                Context.PostReply.Where(x => x.PostId == postId).ExecuteDelete();
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 发帖数据统计
        /// </summary>
        /// <returns></returns>
        public int GetPostCount()
        {
            return Context.Posts.Count();
        }
        public List<StatisticsModel> GetStatistics()
        {
            List<StatisticsModel> sList = new List<StatisticsModel>();
            int n = Context.PostTypes.Count();
            for (int i = 1; i <= n; i++)
            {
                sList.Add(new StatisticsModel
                {
                    Id = i,
                    Name = Context.PostTypes.FirstOrDefault(x => x.Id == i).PostTypeName,
                    Quantity = Context.Posts.Count(x => x.PostTypeId == i)
                });
            }
            return sList;
        }
    }
}
