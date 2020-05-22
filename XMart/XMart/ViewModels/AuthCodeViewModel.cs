using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using XMart.ResponseData;
using XMart.Util;
using XMart.Services;
using System.Threading.Tasks;
using XMart.Models;
using XMart.Views;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Linq;

namespace XMart.ViewModels
{
    public class AuthCodeViewModel : BaseViewModel
    {
        private string authCode;   //Comment
        public string AuthCode
        {
            get { return authCode; }
            set { SetProperty(ref authCode, value); }
        }

        private string secretTel;   //Comment
        public string SecretTel
        {
            get { return secretTel; }
            set { SetProperty(ref secretTel, value); }
        }

        private string remainingTime;   //Comment
        public string RemainingTime
        {
            get { return remainingTime; }
            set { SetProperty(ref remainingTime, value); }
        }

        private bool authCodeButtonEnable;   //Comment
        public bool AuthCodeButtonEnable
        {
            get { return authCodeButtonEnable; }
            set { SetProperty(ref authCodeButtonEnable, value); }
        }

        private string authLoginButtonColor;   //Comment
        public string AuthLoginButtonColor
        {
            get { return authLoginButtonColor; }
            set { SetProperty(ref authLoginButtonColor, value); }
        }

        private string Tel { get; set; }
        public MyTimer myTimer { get; set; }
        //RestSharpService _restSharpService = new RestSharpService();

        public Command LoginCommand { get; set; }
        public Command CheckInputCommand { get; set; }

        public AuthCodeViewModel(string tel)
        {
            Tel = tel;
            AuthCode = string.Empty;
            SecretTel = Tel.Substring(0, 3) + "****" + Tel.Substring(7, 4);
            AuthCodeButtonEnable = false;
            AuthLoginButtonColor = "#83d7f9";

            SendAuthCode();

            LoginCommand = new Command(() =>
            {
                OnLogin();

            }, () => { return true; });

            CheckInputCommand = new Command(() =>
            {
                if (AuthCode.Length == 6)
                {
                    AuthCodeButtonEnable = true;
                    AuthLoginButtonColor = "#01acf2";
                }
            }, () => { return true; });

        }

        public AuthCodeViewModel(RegisterByOpenIdPara registerByOpenIdPara)
        {
            Tel = registerByOpenIdPara.tel;
            AuthCode = string.Empty;
            SecretTel = Tel.Substring(0, 3) + "****" + Tel.Substring(7, 4);
            AuthCodeButtonEnable = false;
            AuthLoginButtonColor = "#83d7f9";

            SendAuthCode();

            LoginCommand = new Command(() =>
            {
                RebindingWechat(registerByOpenIdPara);
            }, () => { return true; });

            CheckInputCommand = new Command(() =>
            {
                if (AuthCode.Length == 6)
                {
                    AuthCodeButtonEnable = true;
                    AuthLoginButtonColor = "#01acf2";
                }
            }, () => { return true; });

        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="registerByOpenIdPara"></param>
        private async void RebindingWechat(RegisterByOpenIdPara registerByOpenIdPara)
        {
            try
            {
                registerByOpenIdPara.authCode = AuthCode;
                SimpleRD simpleRD = await RestSharpService.UpdateWechat(registerByOpenIdPara);

                if (simpleRD.success)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("绑定成功！", ToastLength.Long);

                    LoginRD loginRD = await RestSharpService.LoginByOpenId(registerByOpenIdPara.openId);
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
                else
                {
                    CrossToastPopUp.Current.ShowToastError("绑定失败，请稍后再试！", ToastLength.Long);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 响应发送验证码
        /// </summary>
        private async void SendAuthCode()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                SimpleRD simpleRD = await RestSharpService.SendAuthCode(Tel);

                if (simpleRD.code == 200)
                {
                    myTimer = new MyTimer { EndDate = DateTime.Now.Add(new TimeSpan(900000000)) };
                    LoadAsync();
                    CrossToastPopUp.Current.ShowToastSuccess("请注意查收短信！", ToastLength.Long);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(simpleRD.message, ToastLength.Long);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
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

                if (AuthCode.Length != 6)
                {
                    CrossToastPopUp.Current.ShowToastError("请输入6位验证码", ToastLength.Long);
                    return;
                }

                LoginPara loginPara = new LoginPara
                {
                    authCode = AuthCode,
                    tel = Tel
                };

                LoginRD loginRD = await RestSharpService.LoginByAuthCode(loginPara);

                /*
                if (loginRD.code == 200)
                {
                    CrossToastPopUp.Current.ShowToastSuccess(loginRD.message, ToastLength.Long);

                    MainPage mainPage = new MainPage();
                    await Navigation.PushAsync(mainPage);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(loginRD.message, ToastLength.Long);
                }*/
                if (loginRD.success)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("欢迎您登录美而好家具！", ToastLength.Long);

                    GlobalVariables.LoggedUser = loginRD.result;   //将登录用户的信息保存成全局静态变量
                    GlobalVariables.IsLogged = true;

                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.dat");
                    JObject log = new JObject();
                    log.Add("LoginTime", DateTime.UtcNow);
                    log.Add("UserInfo", JsonConvert.SerializeObject(loginRD.result));
                    //string text = "State:Checked\n" + "Account:" + Tel + "\nPassword:" + loginRD.result + "\nLoginTime:" + DateTime.UtcNow;
                    File.WriteAllText(fileName, log.ToString());

                    //List<Page> pageList = Application.Current.MainPage.Navigation.NavigationStack.ToList<Page>();
                    Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[0]);
                    MainPage mainPage = new MainPage();
                    await Application.Current.MainPage.Navigation.PushAsync(mainPage);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(loginRD.message, ToastLength.Long);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region 计时器

        public void LoadAsync()
        {
            //IsEnable = false;
            //ButtonColor = Color.LightGray;
            myTimer.Start();
            myTimer.Ticked += OnCountdownTicked;
            myTimer.Completed += OnCountdownCompleted;
            //return Task.CompletedTask;
        }

        public Task UnloadAsync()
        {
            myTimer.Ticked -= OnCountdownTicked;
            myTimer.Completed -= OnCountdownCompleted;
            return Task.CompletedTask;
        }

        void OnCountdownTicked()
        {
            RemainingTime = string.Format("{0}", myTimer.RemainTime.TotalSeconds.ToString().Split('.')[0]);
        }

        void OnCountdownCompleted()
        {
            //AuthCodeButtonEnable = true;
            //ButtonColor = Color.FromHex("FFCC00");
            UnloadAsync();
        }

        #endregion
    }
}
