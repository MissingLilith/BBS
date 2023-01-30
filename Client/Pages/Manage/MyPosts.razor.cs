using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Manage
{
    public partial class MyPosts
    {
        [Inject]
        Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Items = Foo.GenerateFoo(Localizer);

            //string token = localStorageService.GetItem<string>("token");
            //if (!string.IsNullOrEmpty(token))
            //{
            //    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //    var posts = await httpClient.GetFromJsonAsync<SelfPostsModel>("api/Post/GetSelfPosts");
            //}
        }
        /// <summary>
        /// Foo 类为Demo测试用，如有需要请自行下载源码查阅
        /// Foo class is used for Demo test, please download the source code if necessary
        /// https://gitee.com/LongbowEnterprise/BootstrapBlazor/blob/main/src/BootstrapBlazor.Shared/Data/Foo.cs
        /// </summary>
        private List<Foo>? Items { get; set; }

        private static IEnumerable<int> PageItemsSource => new int[] { 10, 20, 40, 80, 100 };
        private Task<QueryData<Foo>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<Foo> items = Items;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            return Task.FromResult(new QueryData<Foo>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            });
        }
    }
}
