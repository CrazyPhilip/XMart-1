using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Themes;
using XMart.Util;
using XMart.Services;
using Plugin.Toast;
using Plugin.Toast.Abstractions;

namespace XMart.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private bool darkModeIsToggled;   //Comment
        public bool DarkModeIsToggled
        {
            get { return darkModeIsToggled; }
            set { SetProperty(ref darkModeIsToggled, value); }
        }

        public Command ThemeCommand { get; set; }
        public Command ClearCacheCommand { get; set; }


        public SettingViewModel()
        {
            DarkModeIsToggled = GlobalVariables.DarkMode;
            
            ThemeCommand = new Command(() =>
            {
                GlobalVariables.DarkMode = DarkModeIsToggled;
                Theme theme = GlobalVariables.DarkMode ? Theme.Dark : Theme.Light;

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();

                    switch (theme)
                    {
                        case Theme.Dark:
                            mergedDictionaries.Add(new DarkTheme());
                            break;
                        case Theme.Light:
                        default:
                            mergedDictionaries.Add(new LightTheme());
                            break;
                    }
                }
            }, () => { return true; });

            ClearCacheCommand = new Command(async () =>
            {
                LocalDatabaseService localDatabaseService = new LocalDatabaseService();
                int total = await localDatabaseService.ClearAllData();
                if (total > 0)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("清理完成，共清理" + total.ToString() + "条数据", ToastLength.Long);
                }
            }, () => { return true; });

        }
    }
}
