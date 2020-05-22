using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.Util;
using XMart.Views;
using XMart.ResponseData;
using XMart.Services;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace XMart.ViewModels
{
    public class BindingWechatViewModel : BaseViewModel
    {
        private string avatar;   //Comment
        public string Avatar
        {
            get { return avatar; }
            set { SetProperty(ref avatar, value); }
        }

        private string nickName;   //Comment
        public string NickName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }

        public Command BindingWechatCommand { get; set; }
        public Command ToRegisterCommand { get; set; }
        public Command LoginCommand { get; set; }

        public BindingWechatViewModel(RegisterByOpenIdPara registerByOpenIdPara)
        {
            Avatar = registerByOpenIdPara.headimgurl;
            NickName = registerByOpenIdPara.nikename;

            BindingWechatCommand = new Command(async () =>
            {
                try
                {
                    string result = await Application.Current.MainPage.DisplayPromptAsync("已注册手机号", "请输入11位手机号", "确定绑定", "取消", "请输入11位手机号", 11, null);

                    if (result == null)
                    {
                        CrossToastPopUp.Current.ShowToastWarning("已取消", ToastLength.Long);
                    }
                    else if (string.IsNullOrWhiteSpace(result))
                    {
                        CrossToastPopUp.Current.ShowToastWarning("手机号不能为空，请输入！", ToastLength.Long);
                    }
                    else if (!Tools.IsPhoneNumber(result))
                    {
                        CrossToastPopUp.Current.ShowToastWarning("手机号格式不标准，请检查。", ToastLength.Long);
                    }
                    else
                    {
                        registerByOpenIdPara.tel = result;

                        AuthCodePage authCodePage = new AuthCodePage(registerByOpenIdPara);
                        await Application.Current.MainPage.Navigation.PushAsync(authCodePage);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, () => { return true; });

            ToRegisterCommand = new Command(async () =>
            {
                RegisterPage registerPage = new RegisterPage(registerByOpenIdPara);
                await Application.Current.MainPage.Navigation.PushAsync(registerPage);
            }, () => { return true; });

            LoginCommand = new Command(() =>
            {
                Login(registerByOpenIdPara);
            }, () => { return true; });

        }

        private async void Login(RegisterByOpenIdPara registerByOpenIdPara)
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                LoginRD loginRD = await RestSharpService.LoginByOpenId(registerByOpenIdPara.openId);
                if (loginRD.result.message == "未被注册")
                {
                    CrossToastPopUp.Current.ShowToastError("未注册或未绑定", ToastLength.Long);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastSuccess("欢迎您登录美而好家具！", ToastLength.Long);

                    GlobalVariables.LoggedUser = loginRD.result;   //将登录用户的信息保存成全局静态变量
                    GlobalVariables.IsLogged = true;

                    JObject log = new JObject();
                    log.Add("LoginTime", DateTime.UtcNow);
                    log.Add("UserInfo", JsonConvert.SerializeObject(loginRD.result));
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.dat");
                    File.WriteAllText(fileName, log.ToString());

                    MainPage mainPage = new MainPage();
                    await Application.Current.MainPage.Navigation.PushAsync(mainPage);
                    Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
