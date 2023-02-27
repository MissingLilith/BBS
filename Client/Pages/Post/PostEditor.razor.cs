using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Post
{
    public partial class PostEditor
    {
        [Inject]
        Blazored.LocalStorage.ILocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        SwalService Swal { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "postId")]
        public int PostId { get; set; }
        [NotNull]
        private Markdown? MarkdownSetValue { get; set; }
        private PostDetailModel Post = new();
        private IEnumerable<SelectedItem> Items { get; set; } = new[]
    {
        new SelectedItem ("0", "请选择..."),
        new SelectedItem ("1", "校内新闻"),
        new SelectedItem ("2", "国内新闻"),
        new SelectedItem ("3", "国际新闻"),
        new SelectedItem ("4", "学术交流"),
        new SelectedItem ("5", "同学广场"),
    };
        SwalOption ok = new()
        {
            Category = SwalCategory.Success,
            Title = "发布成功",
            Content = "您的帖子已经发布",
            ShowClose = false,
            IsAutoHide = true,
            Delay = 3000
        };
        SwalOption bad = new()
        {
            Category = SwalCategory.Error,
            Title = "发布失败",
            Content = "Error",
            ShowClose = true
        };
        protected override async Task OnInitializedAsync()
        {
            Post = await httpClient.GetFromJsonAsync<PostDetailModel>($"api/Post/GetPostDetail?postId={PostId}");
            await Task.Delay(600);
            await MarkdownSetValue.SetValue(Post.PostContent);

            await base.OnInitializedAsync();
        }
        private async Task onEdit()
        {
            if (Post.PostTypeId == 0)
            {
                throw new Exception("请选择帖子分区");
            }
            if (string.IsNullOrEmpty(Post.PostTitle))
            {
                throw new Exception("标题不能为空");
            }
            if (string.IsNullOrEmpty(Post.PostContent))
            {
                throw new Exception("内容不能为空");
            }
            string token = await localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var res = await httpClient.PostAsJsonAsync<PostDetailModel>("api/Post/EditPostDetail", Post);
                if (res.IsSuccessStatusCode)
                {
                    await Swal.Show(ok);
                    await Task.Delay(1500);
                    Navigation.NavigateTo("/");
                }
                else
                {
                    bad.Content = await res.Content.ReadAsStringAsync();
                    await Swal.Show(bad);
                }
            }
            else
            {
                Navigation.NavigateTo("login");
                throw new Exception("请先登录");
            }
        }
    }
}
