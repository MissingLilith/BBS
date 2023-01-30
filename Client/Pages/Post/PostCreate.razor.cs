using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Post
{
    public partial class PostCreate
    {
        [Inject]
        Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        SwalService Swal { get; set; }

        private PostCreateModel post = new();
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
            Content = "您可以继续发布新帖",
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
        private async Task onCreate()
        {
            if(post.PostTypeId==0)
            {
                throw new Exception("请选择帖子分区");
            }
            string token = localStorageService.GetItem<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var res = await httpClient.PostAsJsonAsync<PostCreateModel>("api/Post/AddPost", post);
                if(res.IsSuccessStatusCode)
                {
                    await Swal.Show(ok);
                    await Task.Delay(1500);
                    Navigation.NavigateTo("postcreate");
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
