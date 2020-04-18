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



        public Command AddOrUpdateAddressCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command PickPhotoCommand { get; set; }

        public EditUserInfoViewModel()
        {
            //User = GlobalVariables.LoggedUser;
            //Avatar = GlobalVariables.LoggedUser.pricture;
            GenderList = new List<string> { "男", "女", "保密" };

            InitUserInfo();

            AddOrUpdateAddressCommand = new Command(() =>
            {
                
            }, () => { return true; });

            BackCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }, () => { return true; });

            PickPhotoCommand = new Command(async () =>
            {
                Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                if (stream != null)
                {
                    Avatar = ImageSource.FromStream(() => stream);
                }
            }, () => { return true; });

        }

        private async void InitUserInfo()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                RestSharpService _restSharpService = new RestSharpService();
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
    }
}
