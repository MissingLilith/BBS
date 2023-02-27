﻿using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolBBS.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages
{
    public partial class DomesticNews
    {
        private List<PostListModel> Posts = new();

        private static IEnumerable<int> PageItemsSource => new int[] { 10, 20, 40, 80, 100 };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Posts = await httpClient.GetFromJsonAsync<List<PostListModel>>("api/Post/GetPostsByTypeId?typeId=2");
        }
        private Task<QueryData<PostListModel>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<PostListModel> items = Posts;
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
        private void ToCreate()
        {
            string token = localStorageService.GetItem<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo("login");
                throw new Exception("请先登录");
            }
            else
            {
                Navigation.NavigateTo("postcreate");
            }
        }
    }
}
