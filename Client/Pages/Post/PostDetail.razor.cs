using BootstrapBlazor.Components;

namespace SchoolBBS.Client.Pages.Post
{
    public partial class PostDetail
    {
        /// <summary>
        /// Foo 类为Demo测试用，如有需要请自行下载源码查阅
        /// Foo class is used for Demo test, please download the source code if necessary
        /// https://gitee.com/LongbowEnterprise/BootstrapBlazor/blob/main/src/BootstrapBlazor.Shared/Data/Foo.cs
        /// </summary>
        private List<Foo>? Items { get; set; }

        private static IEnumerable<int> PageItemsSource => new int[] { 10, 20, 40, 80, 100 };
        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //获取随机数据
            //Get random data
            Items = Foo.GenerateFoo(Localizer);

        }
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
