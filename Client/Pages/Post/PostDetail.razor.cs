using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Post
{
    public partial class PostDetail
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "postId")]
        public int PostId { get; set; }
        private bool isLiked { get; set; } = false;
        [NotNull]
        private Markdown? MarkdownSetValue { get; set; }
        private PostDetailModel Post = new();
        private List<GetReplysModel> Replys = new();
        private ReplyModel NewReply = new();
        private GetReplysModel ClickReply = new();

        protected override async Task OnInitializedAsync()
        {
            Post = await httpClient.GetFromJsonAsync<PostDetailModel>($"api/Post/GetPostDetail?postId={PostId}");
            Replys = await httpClient.GetFromJsonAsync<List<GetReplysModel>>($"api/Reply/GetReplysByPost?postId={PostId}");
            await Task.Delay(600);
            await MarkdownSetValue.SetValue(Post.PostContent);

            await base.OnInitializedAsync();
        }
        private Task<QueryData<GetReplysModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<GetReplysModel> items = Replys;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            return Task.FromResult(new QueryData<GetReplysModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private async Task OnReply()
        {
            string token = await localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo("login");
                throw new Exception("请先登录");
            }
            if (string.IsNullOrEmpty(NewReply.ReplyContent))
            {
                throw new Exception("内容不能为空");
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                NewReply.PostId = PostId;
                var res = await httpClient.PostAsJsonAsync<ReplyModel>("api/Reply/PostReply", NewReply);
                if (res.IsSuccessStatusCode)
                {
                    await ToastService.Success("回复成功", "请刷新查看", autoHide: true);
                    StateHasChanged();
                }
                else
                {
                    await ToastService.Error("回复失败", res.Content.ReadAsStringAsync().ToString(), autoHide: true);
                }
            }
        }
        private async Task OnLiked(object row)
        {
            string token = await localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo("login");
                throw new Exception("请先登录");
            }
            if (isLiked)
            {
                await ToastService.Error("点赞失败", "你已经点过赞了", autoHide: true);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var res = await httpClient.GetAsync($"api/Post/PostLiked?postId={PostId}");
                if (res.IsSuccessStatusCode)
                {
                    isLiked = true;
                    await ToastService.Success("点赞成功", "感谢你的点赞", autoHide: true);
                }
            }
        }
        private async Task ReplyLiked(GetReplysModel item)
        {
            string token = await localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo("login");
                throw new Exception("请先登录");
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var res = await httpClient.GetAsync($"api/Reply/ReplyLiked?replyId={item.Id}");
                if (res.IsSuccessStatusCode)
                {
                    await ToastService.Success("点赞成功", "感谢你的点赞", autoHide: true);
                }
            }
        }
    }
}
