using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.Services;
using XMart.Util;
using XMart.ResponseData;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace XMart.ViewModels
{
    public class EditUserInfoViewModel : BaseViewModel
    {
        private UserInfo user;   //Comment
        public UserInfo User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private ImageSource avatar;   //Comment
        public ImageSource Avatar
        {
            get { return avatar; }
            set { SetProperty(ref avatar, value); }
        }

        private List<string> genderList;   //Comment
        public List<string> GenderList
        {
            get { return genderList; }
            set { SetProperty(ref genderList, value); }
        }

        private string gender;   //Comment
        public string Gender
        {
            get { return gender; }
            set { SetProperty(ref gender, value); }
        }

        private string password;   //Comment
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string city;   //Comment
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        private string province;   //
        public string Province
        {
            get { return province; }
            set { SetProperty(ref province, value); }
        }

        private string district;   //Comment
        public string District
        {
            get { return district; }
            set { SetProperty(ref district, value); }
        }

        private string remainingTime;   //Comment
        public string RemainingTime
        {
            get { return remainingTime; }
            set { SetProperty(ref remainingTime, value); }
        }

        private Stream imageStream { get; set; }
        private MyTimer myTimer { get; set; }
        private string base64 { get; set; }

        public Command BackCommand { get; set; }
        public Command PickPhotoCommand { get; set; }
        public Command SaveCommand { get; set; }

        RestSharpService _restSharpService = new RestSharpService();

        public EditUserInfoViewModel()
        {
            //User = GlobalVariables.LoggedUser;
            //Avatar = GlobalVariables.LoggedUser.pricture;
            GenderList = new List<string> { "男", "女", "保密" };
            base64 = string.Empty;

            InitUserInfo();

            BackCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }, () => { return true; });

            PickPhotoCommand = new Command(async () =>
            {
                imageStream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                if (imageStream != null)
                {
                    Avatar = ImageSource.FromStream(() => imageStream);
                    //byte[] bytes = new byte[imageStream.Length];
                    //imageStream.Read(bytes, 0, bytes.Length);
                    //imageStream.Seek(0, SeekOrigin.Begin);
                    //base64 = Convert.ToBase64String(bytes);
                }
            }, () => { return true; });

            SaveCommand = new Command(() =>
            {
                SaveUserInfo();
            }, () => { return true; });


        }

        /// <summary>
        /// 保存
        /// </summary>
        private async void SaveUserInfo()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                if ( myTimer != null && myTimer.RemainTime.TotalSeconds > 0)
                {

                }
                else
                {
                    //SimpleRD sendAuthCodeRD = await _restSharpService.SendAuthCode(GlobalVariables.LoggedUser.phone);
                    //myTimer = new MyTimer { EndDate = DateTime.Now.Add(new TimeSpan(900000000)) };
                    //LoadAsync();
                    //
                    //if (sendAuthCodeRD.success)
                    //{
                    //    CrossToastPopUp.Current.ShowToastSuccess("请注意查收短信！", ToastLength.Short);
                    //}
                    //else
                    //{
                    //    CrossToastPopUp.Current.ShowToastWarning("错误，请稍后再试。", ToastLength.Short);
                    //}
                }

                string authCode = await Application.Current.MainPage.DisplayPromptAsync("修改用户信息", string.Format( "请输入验证码：({0}s后重试)", RemainingTime), "确认", "取消", "6位验证码", 6, Keyboard.Numeric);
                
                if (authCode == null)
                {
                    //CrossToastPopUp.Current.ShowToastWarning("已取消", ToastLength.Long);
                    return;
                }
                else if (authCode == "")
                {
                    CrossToastPopUp.Current.ShowToastWarning("请输入验证码。", ToastLength.Long);
                    return;
                }

                //SimpleRD checkAuthCodeRD = await _restSharpService.CheckAuthCode(GlobalVariables.LoggedUser.phone.ToString(), authCode);
                //if (!checkAuthCodeRD.success)
                //{
                //    CrossToastPopUp.Current.ShowToastError("验证码错误。", ToastLength.Short);
                //    return;
                //}

                UpdateUserPara updateUserPara = new UpdateUserPara
                {
                    id = User.id.ToString(),
                    phone = GlobalVariables.LoggedUser.phone,
                    password = Password,
                    email = User.email,
                    username = User.username,
                    description = User.description
                };

                switch (Gender)
                {
                    case "男": updateUserPara.sex = "1"; break;
                    case "女": updateUserPara.sex = "0"; break;
                    case "保密": updateUserPara.sex = "-1"; break;
                    default: updateUserPara.sex = "-1"; break;
                }

                SimpleRD updateUserInfoRD = await _restSharpService.UpdateUserInfo(updateUserPara);
                SimpleRD uploadImageRD = new SimpleRD();

                if (imageStream != null)
                {
                    string base64 = string.Empty;
                    if (imageStream.CanSeek)
                    {
                        byte[] bytes = new byte[imageStream.Length];
                        imageStream.Read(bytes, 0, bytes.Length);
                        imageStream.Seek(0, SeekOrigin.Begin);
                        base64 = Convert.ToBase64String(bytes);
                    }
                    else
                    {
                        //imageStream.Dispose();
                        
                        MemoryStream ms = new MemoryStream();
                        imageStream.CopyTo(ms);
                        byte[] bytes = ms.ToArray();

                        //BinaryFormatter formatter = new BinaryFormatter();
                        //MemoryStream memoryStream = new MemoryStream();
                        //formatter.Serialize(memoryStream, Avatar);
                        //BinaryReader reader = new BinaryReader(memoryStream);
                        //byte[] bytes = new byte[memoryStream.Length];
                        //reader.Read(bytes, 0, bytes.Length);
                        base64 = Convert.ToBase64String(bytes);
                    }

                    uploadImageRD = await _restSharpService.UploadImage(base64);
                }

                if (updateUserInfoRD.success && uploadImageRD.success)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("修改成功！", ToastLength.Short);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
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

        /// <summary>
        /// 初始化
        /// </summary>
        private async void InitUserInfo()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                LoginRD loginRD = await _restSharpService.GetUserInfo();

                if (loginRD.result != null)
                {
                    User = loginRD.result;
                }

                Avatar = User.pricture;
                switch (User.sex)
                {
                    case "0":Gender = "女";break;
                    case "1": Gender = "男"; break;
                    case "-1": Gender = "保密"; break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 计时器

        #endregion
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
    }
}
