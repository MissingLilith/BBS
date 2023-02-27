using BootstrapBlazor.Components;
using SchoolBBS.Shared;
using System.Linq;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SchoolBBS.Client.Pages.Manage
{
    public partial class UserManage
    {
        private List<UserManageModel> Item = new();
        //private static IEnumerable<int> PageItemsSource => new int[] { 1, 2, 3, 15, 20 };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string token = await localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Item = await httpClient.GetFromJsonAsync<List<UserManageModel>>("api/Account/GetAllUsers");
            }
        }
        private Task<QueryData<UserManageModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<UserManageModel> items = Item;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<UserManageModel>()
            {
                Items = items,
                TotalCount = total,
            });
        }
        private static Task<UserManageModel> OnAddAsync() => Task.FromResult(new UserManageModel() { });

        private async Task<bool> OnSaveAsync(UserManageModel item,ItemChangedType itemChangedType)
        {
            var res = await httpClient.PostAsJsonAsync("api/Account/EditUserByAdmin", item);
            if (res.IsSuccessStatusCode)
            {
                if (item.UserId == 0)
                {
                    item.UserId = await res.Content.ReadFromJsonAsync<int>();
                    item.CreateTime= DateTime.Now;
                    Item.Add(item);
                }
                else
                {
                    Item.ForEach(i =>
                {
                    if (i.UserId == item.UserId)
                    {
                        i.UserName = item.UserName;
                        i.UserPassword = item.UserPassword;
                        i.IsAdmin = item.IsAdmin;
                    }
                });
                }
                return await Task.FromResult(true);
            }
            else { return false; }
        }
        private Task<bool> OnDeleteAsync(IEnumerable<UserManageModel> items)
        {
            items.ToList().ForEach(async i => await httpClient.DeleteAsync($"api/Account/DeleteUser?userId={i.UserId}"));
            items.ToList().ForEach(i => Item.Remove(i));
            return Task.FromResult(true);
        }
    }
}
