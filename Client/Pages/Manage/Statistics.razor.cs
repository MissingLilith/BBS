using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace SchoolBBS.Client.Pages.Manage
{
    public partial class Statistics
    {
        [Inject]
        Blazored.LocalStorage.ILocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [NotNull]
        private Chart? DoughnutChart { get; set; }
        private List<StatisticsModel> statisticsModelList = new List<StatisticsModel>();
        private int UserCount { get; set; }
        private int UserCountV { get; set; }
        private int PostCount { get; set; }
        private int PostCountV { get; set; }
        private int ReplyCount { get; set; }
        private int ReplyCountV { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string token = await localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                UserCount = Convert.ToInt32(await httpClient.GetStringAsync("api/Account/GetUserCount"));
                PostCount = Convert.ToInt32(await httpClient.GetStringAsync("api/Post/GetPostCount"));
                ReplyCount = Convert.ToInt32(await httpClient.GetStringAsync("api/Reply/GetReplyCount"));
                float u = (float)UserCount / ((float)UserCount + (float)PostCount + (float)ReplyCount) * 100;
                float p = (float)PostCount / ((float)UserCount + (float)PostCount + (float)ReplyCount) * 100;
                float r = (float)ReplyCount / ((float)UserCount + (float)PostCount + (float)ReplyCount) * 100;
                UserCountV= Convert.ToInt32(u);
                PostCountV= Convert.ToInt32(p);
                ReplyCountV= Convert.ToInt32(r);
            }
        }
        private async Task<ChartDataSource> OnInit()
        {
            statisticsModelList= await httpClient.GetFromJsonAsync<List<StatisticsModel>>("api/Post/GetStatistics");
            var ds = new ChartDataSource();
            ds.Options.Title = "帖子数据统计";
            ds.Labels = statisticsModelList.Select(x=>x.Name).ToList();
                ds.Data.Add(new ChartDataset()
                {
                    Label = "数量",
                    Data = statisticsModelList.Select(x=>x.Quantity).ToList().Cast<object>()
                });
            return await Task.FromResult(ds);
        }
    }
}
