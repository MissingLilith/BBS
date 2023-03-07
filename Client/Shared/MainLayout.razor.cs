using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace SchoolBBS.Client.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainLayout
    {
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }
        private string? UserName { get; set; }
        private bool IsLogin { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

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
                var info = await httpClient.GetFromJsonAsync<GetUserModel>("api/Account/GetSelfInfo");
                UserName = info.UserName;
                IsAdmin= info.IsAdmin;
                IsLogin = true;
            }
        }

        private static List<MenuItem> GetIconSideMenuItems()
        {
            var menus = new List<MenuItem>
        {
            new MenuItem() { Text = "主页", Icon = "fa-solid fa-fw fa-home", Url = "/" , Match = NavLinkMatch.All},
            new MenuItem() { Text = "新鲜出炉", Icon = "fa-solid fa-fw fa-tag", Url = "newposts" },
            new MenuItem() { Text = "人气热门", Icon = "fa-solid fa-fw fa-fire", Url = "hotposts" },
            new MenuItem() { Text = "校内新闻", Icon = "fa-solid fa-fw fa-school", Url = "schoolnews" },
            new MenuItem() { Text = "国内新闻", Icon = "fa-solid fa-fw fa-newspaper", Url = "domesticnews" },
            new MenuItem() { Text = "国际新闻", Icon = "fa-solid fa-fw fa-earth-asia", Url = "internationalnews" },
            new MenuItem() { Text = "学术交流", Icon = "fa-solid fa-fw fa-book", Url = "academicexchange" },
            new MenuItem() { Text = "同学广场", Icon = "fa-solid fa-fw fa-comments", Url = "studentcomments" }
        };

            return menus;
        }

        [Inject] Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject] HttpClient httpClient { get; set; }

    }
}