using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using SchoolBBS.Shared;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SchoolBBS.Client.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class ManageLayout
    {
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }
        private bool IsAdmin { get; set; }
        private string UserName { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            Menus = GetIconSideMenuItems();
            string token = localStorageService.GetItem<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var info = await httpClient.GetFromJsonAsync<GetUserModel>("api/Account/GetSelfInfo");
                    UserName = info.UserName;
                    IsAdmin = info.IsAdmin;
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
            new MenuItem() { Text = "用户管理", Icon = "fa-solid fa-fw fa-user-gear", Url = "usermanage" },
            new MenuItem() { Text = "帖子管理", Icon = "fa-solid fa-fw fa-file-lines", Url = "postmanage" },
            new MenuItem() { Text = "回复管理", Icon = "fa-solid fa-fw fa-comment-dots", Url = "replymanage" },
            new MenuItem() { Text = "数据统计", Icon = "fa-solid fa-fw fa-chart-simple", Url = "statistics" }
        };

            return menus;
        }
        [Inject] Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject] HttpClient httpClient { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        private void ButtonClick(string s)
        {
            Navigation.NavigateTo(s);
        }
    }
}