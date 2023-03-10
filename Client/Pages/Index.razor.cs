using BootstrapBlazor.Components;
using SchoolBBS.Shared;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages
{
    public partial class Index
    {
        /// <summary>
        /// Images
        /// </summary>
        private static List<string> Images => new()
    {
        "/images/banner1.jpg",
        "/images/banner2.png",
        "/images/banner3.jpg"
    };

        private List<PostListModel> Item = new();
        private List<PostListModel> Item2 = new();
        private static IEnumerable<int> PageItemsSource => new int[] { 7 };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Item = await httpClient.GetFromJsonAsync<List<PostListModel>>("api/Post/GetPostByTime");
            Item2 = await httpClient.GetFromJsonAsync<List<PostListModel>>("api/Post/GetPostByLikes");
        }
        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private Task<QueryData<PostListModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<PostListModel> items = PostsByTime;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<PostListModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private Task<QueryData<PostListModel>> OnQueryAsync2(QueryPageOptions options)
        {
            IEnumerable<PostListModel> items = PostsByLikes;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<PostListModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private Task<QueryData<PostListModel>> OnQueryAsync2(QueryPageOptions options)
        {
            IEnumerable<PostListModel> items = Item2;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<PostListModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        /// <summary>
        /// 按钮点击时间
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task ClickRow(PostListModel item)
        {
            Navigation.NavigateTo($"/postdetail?postId={item.Id}");
            return Task.CompletedTask;
        }
    }
}
