using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Manage
{
    public partial class PostManage
    {
        private List<PostManageModel> Item = new();
        //private static IEnumerable<int> PageItemsSource => new int[] { 1, 2, 3, 15, 20 };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string token = await localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Item = await httpClient.GetFromJsonAsync<List<PostManageModel>>("api/Post/GetAllPosts");
            }
        }
        private Task<QueryData<PostManageModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<PostManageModel> items = Item;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<PostManageModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private static Task<PostManageModel> OnAddAsync() => Task.FromResult(new PostManageModel() { });

        private async Task<bool> OnSaveAsync(PostManageModel item, ItemChangedType itemChangedType)
        {
            var res = await httpClient.PostAsJsonAsync("api/Post/EditPost", item);
            if (res.IsSuccessStatusCode)
            {
                Item.ForEach(i =>
                {
                    if (i.Id == item.Id)
                    {
                        i.PostTitle = item.PostTitle;
                        i.PostTypeId = item.PostTypeId;
                    }
                });
                return await Task.FromResult(true);
            }
            else { return false; }
        }
        private Task<bool> OnDeleteAsync(IEnumerable<PostManageModel> items)
        {
            items.ToList().ForEach(async i => await httpClient.DeleteAsync($"api/Post/DeletePost?postId={i.Id}"));
            items.ToList().ForEach(i => Item.Remove(i));
            return Task.FromResult(true);
        }
        private Task ClickRow(PostManageModel item)
        {
            Navigation.NavigateTo($"/posteditor?postId={item.Id}");
            return Task.CompletedTask;
        }
    }
}
