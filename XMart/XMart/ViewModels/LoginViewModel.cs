﻿using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using XMart.Views;
using XMart.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace XMart.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string tel;   //手机号
        public string Tel
        {
            get { return tel; }
            set { SetProperty(ref tel, value); }
        }

        private string pwd;   //密码
        public string Pwd
        {
            get { return pwd; }
            set { SetProperty(ref pwd, value); }
        }

        private bool isRememberPwd;   //是否记住密码
        public bool IsRememberPwd
        {
            get { return isRememberPwd; }
            set { SetProperty(ref isRememberPwd, value); }
        }

        private string eyeSource;   //Comment
        public string EyeSource
        {
            get { return eyeSource; }
            set { SetProperty(ref eyeSource, value); }
        }

        private bool isPassword;   //Comment
        public bool IsPassword
        {
            get { return isPassword; }
            set { SetProperty(ref isPassword, value); }
        }

        private bool authVisible;   //Comment
        public bool AuthVisible
        {
            get { return authVisible; }
            set { SetProperty(ref authVisible, value); }
        }

        private bool passwordVisible;   //Comment
        public bool PasswordVisible
        {
            get { return passwordVisible; }
            set { SetProperty(ref passwordVisible, value); }
        }

        private string authLoginButtonColor;   //Comment
        public string AuthLoginButtonColor
        {
            get { return authLoginButtonColor; }
            set { SetProperty(ref authLoginButtonColor, value); }
        }

        public Command ToRegisterPageCommand { get; private set; }   //跳转到注册页面
        public Command LoginCommand { get; private set; }   //登录按钮
        public Command FindPwdCommand { get; private set; }   //跳转到找回密码页面
        public Command OpenEyeCommand { get; private set; }
        public Command ToAuthPageCommand { get; set; }
        public Command PasswordLoginPartCommand { get; set; }
        public Command AuthLoginPartCommand { get; set; }
        public Command WechatLoginCommand { get; set; }

        public LoginViewModel()
        {
            IsPassword = true;
            EyeSource = "Resource/drawable/closed_eye.png";
            AuthLoginButtonColor = "#83d7f9";
            AuthVisible = true;
            PasswordVisible = false;

            ToRegisterPageCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
            }, () => { return true; });

            LoginCommand = new Command(() =>
            {
                if (CheckInput())
                {
                    OnLogin();
                }
            }, () => { return true; });

            FindPwdCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ResetPwdPage());
            }, () => { return true; });

            OpenEyeCommand = new Command(() =>
            {
                if (IsPassword)
                {
                    IsPassword = false;
                    EyeSource = "Resource/drawable/open_eye.png";
                }
                else
                {
                    IsPassword = true;
                    EyeSource = "Resource/drawable/closed_eye.png";
                }
            }, () => { return true; });

            ToAuthPageCommand = new Command(() =>
            {
                if (Tools.IsPhoneNumber(Tel))
                {
                    AuthCodePage authCodePage = new AuthCodePage(Tel);
                    Application.Current.MainPage.Navigation.PushAsync(authCodePage);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastWarning("手机号格式不标准，请检查。", ToastLength.Long);
                }
            }, () => { return true; });

            PasswordLoginPartCommand = new Command(() =>
            {
                AuthVisible = false;
                PasswordVisible = true;
            }, () => { return true; });

            AuthLoginPartCommand = new Command(() =>
            {
                AuthVisible = true;
                PasswordVisible = false;
            }, () => { return true; });

            WechatLoginCommand = new Command(() =>
            {
                MessagingCenter.Send(new object(), "Register");//首先进行注册，然后订阅注册的结果。
                MessagingCenter.Send(new object(), "Login");

                MessagingCenter.Subscribe<object, string>(this, "LoginSuccess", async (sender, result) =>
                {
                    try
                    {
                        JObject jObject = JObject.Parse(result);
                        RegisterByOpenIdPara registerByOpenIdPara = new RegisterByOpenIdPara
                        {
                            openId = jObject["openid"].ToString(),
                            nikename = jObject["nickname"].ToString(),
                            headimgurl = jObject["headimgurl"].ToString(),

                        };

                        BindingWechatPage bindingWechatPage = new BindingWechatPage(registerByOpenIdPara);
                        await Application.Current.MainPage.Navigation.PushAsync(bindingWechatPage);

                        MessagingCenter.Unsubscribe<object, string>(this, "LoginSuccess");
                        MessagingCenter.Unsubscribe<object, string>(this, "Login");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("错误：" + ex);
                    }
                });
            }, () => { return true; });
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(Tel))
            {
                CrossToastPopUp.Current.ShowToastWarning("手机号不能为空，请输入！", ToastLength.Long);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Pwd))
            {
                CrossToastPopUp.Current.ShowToastWarning("密码不能为空，请输入！", ToastLength.Long);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登录
        /// </summary>
        private async void OnLogin()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                LoginPara loginPara = new LoginPara
                {
                    userPwd = Pwd,
                    authCode = "",
                    tel = Tel
                };

                //RestSharpService _restSharpService = new RestSharpService();
                LoginRD loginRD = await RestSharpService.Login(loginPara);

                if (loginRD.result.message == null)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("欢迎您登录美而好家具！", ToastLength.Long);

                    GlobalVariables.LoggedUser = loginRD.result;   //将登录用户的信息保存成全局静态变量
                    GlobalVariables.IsLogged = true;

                    JObject log = new JObject();
                    log.Add("LoginTime", DateTime.UtcNow);
                    log.Add("UserInfo", JsonConvert.SerializeObject(loginRD.result));
                    string fileName = Path.Combine(FileSystem.CacheDirectory, "log.dat");
                    File.WriteAllText(fileName, log.ToString());

                    Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[0]);
                    MainPage mainPage = new MainPage();
                    await Application.Current.MainPage.Navigation.PushAsync(mainPage);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(loginRD.result.message, ToastLength.Long);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
