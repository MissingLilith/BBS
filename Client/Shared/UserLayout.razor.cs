using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Shared
{
    public sealed partial class UserLayout
    {
        [Inject]
        Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }
        private bool IsLogin { get; set; } = false;
        private string UserName { get; set; }
        private bool IsAdmin { get; set; } = false;

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Menus = GetIconSideMenuItems();
            string token = localStorageService.GetItem<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var info = await httpClient.GetFromJsonAsync<GetUserModel>("api/Account/GetSelfInfo");
                    UserName = info.UserName;
                    IsAdmin= info.IsAdmin;
                    IsLogin = true;
                }
                catch (Exception ex)
                {
                    localStorageService.RemoveItem("token");
                    throw new Exception("用户数据获取失败，请重新登陆");
                }
            }
        }

        private static List<MenuItem> GetIconSideMenuItems()
        {
            var menus = new List<MenuItem>
        {
            new MenuItem() { Text = "返回主页", Icon = "fa-solid fa-fw fa-home", Url = "/" , Match = NavLinkMatch.All},
            new MenuItem() { Text = "账号信息", Icon = "fa-solid fa-fw fa-circle-user", Url = "usercenter" },
            new MenuItem() { Text = "我的帖子", Icon = "fa-solid fa-fw fa-file-lines", Url = "myposts" },
            new MenuItem() { Text = "我的回复", Icon = "fa-solid fa-fw fa-comment-dots", Url = "myreply" }
        };

            return menus;
        }

        private void ButtonClick(string s)
        {
            Navigation.NavigateTo(s);
        }
    }
}

