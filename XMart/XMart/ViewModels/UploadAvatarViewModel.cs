using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using XMart.Util;

namespace XMart.ViewModels
{
    public class UploadAvatarViewModel : BaseViewModel
    {
        private ImageSource oldAvatar;   //Comment
        public ImageSource OldAvatar
        {
            get { return oldAvatar; }
            set { SetProperty(ref oldAvatar, value); }
        }

        private ImageSource newAvatar;   //Comment
        public ImageSource NewAvatar
        {
            get { return newAvatar; }
            set { SetProperty(ref newAvatar, value); }
        }

        private string base64 { get; set; }

        public Command PickPhotoCommand { get; set; }
        public Command SaveCommand { get; set; }

        public UploadAvatarViewModel()
        {
            OldAvatar = GlobalVariables.LoggedUser.pricture;
            NewAvatar = "Resource/drawable/plus.png";

            base64 = "data:image/jpeg;base64,";

            PickPhotoCommand = new Command(async () =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    CrossToastPopUp.Current.ShowToastWarning("请打开权限。", ToastLength.Long);
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    //PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 128
                });

                if (file == null)
                    return;

                NewAvatar = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    base64 += Convert.ToBase64String(bytes);
                    file.Dispose();
                    return stream;
                });
            }, () => { return true; });

            SaveCommand = new Command(() =>
            {
                UploadAvatar();
            }, () => { return true; });
        }

        /// <summary>
        /// 上传新头像
        /// </summary>
        private async void UploadAvatar()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                if (string.IsNullOrWhiteSpace(base64))
                {
                    CrossToastPopUp.Current.ShowToastWarning("请选择新头像", ToastLength.Long);
                    return;
                }

                //RestSharpService _restSharpService = new RestSharpService();
                SimpleRD uploadImageRD = await RestSharpService.UploadImage(base64);

                if (uploadImageRD.success)
                {
                    LoginRD loginRD = await RestSharpService.GetUserInfo();
                    if (loginRD.result.message == null)
                    {
                        GlobalVariables.LoggedUser = loginRD.result;   //将登录用户的信息保存成全局静态变量
                        GlobalVariables.IsLogged = true;

                        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.dat");
                        JObject log = new JObject();
                        log.Add("LoginTime", DateTime.UtcNow);
                        log.Add("UserInfo", JsonConvert.SerializeObject(loginRD.result));
                        //string text = "State:Checked\n" + "Account:" + Tel + "\nPassword:" + loginRD.result + "\nLoginTime:" + DateTime.UtcNow;
                        File.WriteAllText(fileName, log.ToString());
                    }

                    CrossToastPopUp.Current.ShowToastSuccess("修改成功！", ToastLength.Short);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError("修改失败，请稍后再试。", ToastLength.Short);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
