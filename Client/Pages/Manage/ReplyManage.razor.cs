using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Manage
{
    public partial class ReplyManage
    {
        [Inject]
        Blazored.LocalStorage.ILocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }

        private List<ReplyManageModel> Item = new();
        //private static IEnumerable<int> PageItemsSource => new int[] { 1, 2, 3, 15, 20 };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string token = await localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Item = await httpClient.GetFromJsonAsync<List<ReplyManageModel>>("api/Reply/GetAllReplys");
            }
        }
        private Task<QueryData<ReplyManageModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<ReplyManageModel> items = Item;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<ReplyManageModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private Task<bool> OnDeleteAsync(IEnumerable<ReplyManageModel> items)
        {
            items.ToList().ForEach(async i => await httpClient.DeleteAsync($"api/Reply/DeleteReply?ReplyId={i.Id}"));
            items.ToList().ForEach(i => Item.Remove(i));
            return Task.FromResult(true);
        }
        private Task ClickRow(ReplyManageModel item)
        {
            Navigation.NavigateTo($"/posteditor?postId={item.Id}");
            return Task.CompletedTask;
        }
    }
}
