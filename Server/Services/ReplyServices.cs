using SchoolBBS.Entity.Models;
using SchoolBBS.Shared;

namespace SchoolBBS.Server.Services
{
    public class ReplyServices
    {
        readonly bbsdbContext Context;
        public ReplyServices(bbsdbContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 获取全部回复
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<PostReply> GetAllReplys()
        {
            List<PostReply> postReplies = Context.PostReply.ToList();
            if (postReplies.Count > 0)
            {
                return postReplies;
            }
            else
            {
                throw new Exception("数据获取失败或无数据");
            }
        }
        /// <summary>
        /// 获取个人回复
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<PostReply> GetSelfReplys(int userId)
        {
            List<PostReply> postReplies=Context.PostReply.Where(x=>x.CreateUserId== userId).ToList();
            if (postReplies.Count > 0)
            {
                return postReplies;
            }
            else { throw new Exception("获取数据失败或无数据"); }
        }
        /// <summary>
        /// 获取帖子回复
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<PostReply> GetReplysByPost(int postId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == postId);
            if (isPostHave)
            {
                return Context.PostReply.Where(x => x.PostId == postId).ToList();
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 回复点赞
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public bool ReplyLiked(int replyId)
        {
            PostReply reply = Context.PostReply.FirstOrDefault(x => x.Id == replyId);
            reply.Likes += 1;
            Context.SaveChanges();
            return true;
        }
        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool PostReply(ReplyModel reply, int userId)
        {
            bool isPostHave = Context.Posts.ToList().Exists(x => x.Id == reply.PostId);
            if (isPostHave)
            {
                PostReply postReply = new();
                postReply.PostId = reply.PostId;
                postReply.ReplyContent = reply.ReplyContent;
                postReply.CreateUserId = userId;
                postReply.CreateTime = DateTime.Now;
                Context.PostReply.Add(postReply);
                Posts post = Context.Posts.FirstOrDefault(x => x.Id == reply.PostId);
                post.LastReplyUserId = userId;
                post.LastReplyTime = DateTime.Now;
                post.Replys += 1;
                Context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("帖子不存在");
            }
        }
        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteReply(int replyId)
        {
            bool isReplyHave = Context.PostReply.ToList().Exists(x => x.Id == replyId);
            if (isReplyHave)
            {
                Context.PostReply.Remove(Context.PostReply.FirstOrDefault(x => x.Id == replyId));
                Context.SaveChanges();
                return true;
            }
            else { throw new Exception("回复不存在"); }
        }
        public int GetReplyCount()
        {
            return Context.PostReply.Count();
        }
        
    }
}
