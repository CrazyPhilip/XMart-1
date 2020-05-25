using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Themes;
using XMart.Util;
using XMart.Services;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System.IO;

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
        public Command UpdateCommand { get; set; }

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

            UpdateCommand = new Command(() =>
            {

            }, () => { return true; });


        }

        /*
        public void run()
        {
            int receivedBytes = 0;
            int totalBytes = 0;
            string dirPath = "/sdcard/updateVersion/version";
            var filePath = Path.Combine(dirPath, "hz_android.apk");
            URL url = new URL(urlToDownload);//urlToDownload 下载文件的url地址

            HttpURLConnection conn = (HttpURLConnection)url.OpenConnection();
            conn.Connect();
            Stream Ins = conn.InputStream;
            totalBytes = conn.ContentLength;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            else
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            using (FileStream fos = new FileStream(filePath, FileMode.Create))
            {
                byte[] buf = new byte[512];

                do
                {
                    int numread = Ins.Read(buf, 0, 512);
                    receivedBytes += numread;
                    if (numread <= 0)
                    {
                        break;
                    }
                    fos.Write(buf, 0, numread);

                    //进度条代码
                    if (progessReporter != null)
                    {
                        DownloadBytesProgress args = new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes);
                        progessReporter.Report(args);
                    }
                } while (true);
            }

            //调用下载的文件进行安装
            installApk(filePath);
        }

        private void installApk(string filePath)
        {
            var context = Xamarin.Forms.Context;
            if (context == null)
                return;
            // 通过Intent安装APK文件
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Uri.Parse("file://" + filePath), "application/vnd.android.package-archive");
            //Uri content_url = Uri.Parse(filePath);
            //intent.SetData(content_url);
            intent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }*/
    }
}
