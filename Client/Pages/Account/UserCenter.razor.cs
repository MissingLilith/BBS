using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SchoolBBS.Shared;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

namespace SchoolBBS.Client.Pages.Account
{
    public partial class UserCenter
    {
        private int UserId { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private DateTime? CreatedTime { get; set; }
        private bool IsAdmin { get; set; }
        private bool IsLoggedIn { get; set; } = false;
        private string OldPwd { get; set; }
        private string NewPwd { get; set; }
        private string PwdConfirm { get; set; }

        [Inject]
        Blazored.LocalStorage.ISyncLocalStorageService localStorageService { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }
        EditUserModel editUserModel = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string token = localStorageService.GetItem<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var info = await httpClient.GetFromJsonAsync<GetUserModel>("api/Account/GetSelfInfo");
                UserId = info.UserId;
                UserName = info.UserName;
                Password = info.UserPassword;
                CreatedTime = info.CreateTime;
                IsAdmin = info.IsAdmin;
                IsLoggedIn = true;
            }
        }
        private async Task ChangeName()
        {
            if (OldPwd == null || OldPwd != Password)
            {
                throw new Exception("请输入正确的原密码");
            }
            if (string.IsNullOrEmpty(editUserModel.UserName))
            {
                throw new Exception("用户名不能为空");
            }
            if (UserName == editUserModel.UserName)
            {
                throw new Exception("不能与当前用户名相同");
            }
            editUserModel.UserPassword = OldPwd;
            var res = await httpClient.PostAsJsonAsync<EditUserModel>("api/Account/EditUserName", editUserModel);
            if (res.IsSuccessStatusCode)
            {
                await ToastService.Success("用户名修改成功", "您的用户名已修改，请刷新网页", autoHide: true);
            }
            else
            {
                await ToastService.Error("修改失败", await res.Content.ReadAsStringAsync(), autoHide: true);
            }
        }
        private async Task ChangePwd()
        {
            editUserModel.UserName = UserName;
            editUserModel.UserPassword = NewPwd;
            if (OldPwd == null || editUserModel.UserPassword == null || PwdConfirm == null)
            {
                throw new Exception("密码不能为空");
            }
            if (OldPwd != Password)
            {
                throw new Exception("原密码输入错误");
            }
            if (NewPwd != PwdConfirm)
            {
                throw new Exception("两次输入的密码不一致");
            }
            if (NewPwd == Password)
            {
                throw new Exception("不能与原密码相同");
            }
            var res = await httpClient.PostAsJsonAsync<EditUserModel>("api/Account/EditUserPassword", editUserModel);
            if (res.IsSuccessStatusCode)
            {
                await ToastService.Success("密码修改成功", "您的密码已修改，请重新登录", autoHide: true);
                await Task.Delay(1500);
                Navigation.NavigateTo("/logout");
            }
            else
            {
                await ToastService.Error("修改失败", await res.Content.ReadAsStringAsync(), autoHide: true);
            }
        }
    }
}
