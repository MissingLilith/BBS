using SchoolBBS.Shared;

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
        /// <summary>
        /// Foo 类为Demo测试用，如有需要请自行下载源码查阅
        /// Foo class is used for Demo test, please download the source code if necessary
        /// https://gitee.com/LongbowEnterprise/BootstrapBlazor/blob/main/src/BootstrapBlazor.Shared/Data/Foo.cs
        /// </summary>
        private List<Foo>? Items { get; set; }

        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //获取随机数据
            //Get random data
            Items = Foo.GenerateFoo(Localizer);
        }

        private void ToPD()
        {
            Navigation.NavigateTo("postdetail");
        }
        private void ToUC()
        {
            Navigation.NavigateTo("usercenter");
        }
        private void ToPC()
        {
            Navigation.NavigateTo("postcreator");
        }
    }
}
