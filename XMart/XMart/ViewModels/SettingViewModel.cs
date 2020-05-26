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
using RestSharp;
using RestSharp.Extensions;
using Xamarin.Essentials;
using System.Threading;

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

        private string rate;   //Comment
        public string Rate
        {
            get { return rate; }
            set { SetProperty(ref rate, value); }
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
                try
                {
                    string[] args = { "baseUrl", "request", "fileName" };
                    Thread the = new Thread(new ParameterizedThreadStart(DownloadApk));
                    the.Start(args);
                }
                catch (ThreadAbortException)
                {
                }

            }, () => { return true; });


        }

        private void DownloadApk(object obj)
        {
            string[] args = obj as string[];
            string tempFile = Path.Combine("/storage/emulated/0/Android/data/com.wyhl.XMart/files/", args[2].ToString());
            //Console.WriteLine(FileSystem.AppDataDirectory);
            using (var writer = new HikFileStream(tempFile))
            {
                RestClient client = new RestClient(args[0])
                {
                    Timeout = 10 * 1000 //10 sec timeout time.
                };

                RestRequest request = new RestRequest(args[1]);
                //request.AddParameter("FileName", "testFile.abc", ParameterType.UrlSegment);

                writer.Progress += (w, e) => {
#if DEBUG
                    //Console.Write(string.Format("\rProgress: {0} / {1:P2}", writer.CurrentSize, ((double)writer.CurrentSize) / finalFileSize));
#endif

                    Rate = string.Format("{0:F2} MB", ((double)writer.CurrentSize / 1048576));
                };

                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = client.DownloadData(request);
            }

            if (File.Exists(tempFile))
            {
                CrossToastPopUp.Current.ShowToastSuccess("下载成功！", ToastLength.Long);
            }
            else
            {
                CrossToastPopUp.Current.ShowToastError("下载失败！", ToastLength.Long);
            }
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

        class HikFileStream : FileStream
        {
            public HikFileStream(string path) : base(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
            {
            }

            public long CurrentSize { get; private set; }

            public event EventHandler Progress;

            public override void Write(byte[] array, int offset, int count)
            {
                base.Write(array, offset, count);
                CurrentSize += count;
                Progress?.Invoke(this, EventArgs.Empty);//WARN: THIS SHOULD RETURNS ASAP!
            }

        }
    }
}
