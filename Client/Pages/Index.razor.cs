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

        private List<PostListModel> PostsByTime = new();
        private List<PostListModel> PostsByLikes = new();
        private static IEnumerable<int> PageItemsSource => new int[] { 7 };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            PostsByTime = await httpClient.GetFromJsonAsync<List<PostListModel>>("api/Post/GetPostByTime");
            PostsByLikes = await httpClient.GetFromJsonAsync<List<PostListModel>>("api/Post/GetPostByLikes");
            await Task.Delay(600);
        }
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
        private Task ClickRow(PostListModel item)
        {
            Navigation.NavigateTo($"/postdetail?postId={item.Id}");
            return Task.CompletedTask;
        }
    }
}
