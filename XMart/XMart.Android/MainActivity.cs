using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using Com.Alipay.Sdk.App;
using Com.Tencent.MM.Opensdk.Modelmsg;
using Com.Tencent.MM.Opensdk.Openapi;
using FFImageLoading.Forms.Platform;
using Java.IO;
using Plugin.CurrentActivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xam.Plugin.WebView.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XMart.Droid
{
    [Activity(MainLauncher = false, Label = "美而好", Icon = "@mipmap/xmart", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        //微信相关
        private readonly string appID = "wxfad74b8fe74b6c22";//申请的appid
        private IWXAPI wxApi;

        //private MyHandler handler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Resources.DisplayMetrics.ScaledDensity = 2;//告诉android不要把自己大小单位缩放
            //double systemDensity = DeviceDisplay.MainDisplayInfo.Density;
            Resources.DisplayMetrics.Density = 2;
            //var temp = Resources.DisplayMetrics;
            //var device = DeviceDisplay.MainDisplayInfo;
            //if (DeviceDisplay.MainDisplayInfo.Density >= 3)
            //{
            //    Resources.DisplayMetrics.ScaledDensity = 1.5F;//告诉android不要把自己大小单位缩放
            //    Resources.DisplayMetrics.Density = 2.5F;
            //}
            //else
            //{
            //    Resources.DisplayMetrics.ScaledDensity = 2;//告诉android不要把自己大小单位缩放
            //    Resources.DisplayMetrics.Density = 2;
            //}
            App.ScreenWidth = Resources.DisplayMetrics.WidthPixels;
            App.ScreenHeight = Resources.DisplayMetrics.HeightPixels;

            //var width = Resources.DisplayMetrics.WidthPixels;
            //var height = Resources.DisplayMetrics.HeightPixels;
            //var density = Resources.DisplayMetrics.Density; //屏幕密度
            //App.ScreenWidth = width / density; //屏幕宽度
            //App.ScreenHeight = height / density; //屏幕高度 含24个单位的标题栏高度 通过OnSizeAllocated获取的高度不含标题栏高度

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                //透明状态栏                
                //Window.AddFlags(WindowManagerFlags.TranslucentStatus);
                Window.SetStatusBarColor(Android.Graphics.Color.LightGray);
                //不遮挡导航栏                
                Window.AddFlags(WindowManagerFlags.ForceNotFullscreen);
            }

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);   //弹出框
            CarouselViewRenderer.Init();    //轮播图
            CachedImageRenderer.Init(true);
            //Forms.SetFlags("CarouselView_Experimental");
            FormsWebViewRenderer.Initialize();
            Forms.SetFlags("SwipeView_Experimental");

            //支付宝
            MessagingCenter.Subscribe<object, string>(this, "Pay", (sender, sign) =>
            {
                try
                {
                    Thread the = new Thread(new ParameterizedThreadStart(Pay));
                    the.Start(sign);
                }
                catch (ThreadAbortException)
                {
                }
            });

            //微信相关
            //注册
            MessagingCenter.Subscribe<object>(this, "Register", d =>
            {
                var result = RegToWx();
            });

            MessagingCenter.Subscribe<object>(this, "Login", d =>
            {
                // send oauth request
                SendAuth.Req req = new SendAuth.Req()
                {
                    Scope = "snsapi_userinfo",
                    State = "xmart_wechat_login"
                };
                bool result = wxApi.SendReq(req);
            });

            //分享小程序给朋友
            MessagingCenter.Subscribe<object, string>(this, "ShareMiniProgramToFriend", (sender, arg) =>
            {
                string[] args = arg.Split(',');
                WXMiniProgramObject miniProgramObj = new WXMiniProgramObject
                {
                    WebpageUrl = "https://www.qq.com", // 兼容低版本的网页链接
                    MiniprogramType = WXMiniProgramObject.MiniprogramTypePreview,// 正式版:0，测试版:1，体验版:2
                    UserName = "gh_8de3642430df",     // 小程序原始id
                    Path = "/pages/detail/detail?productId=" + args[0] + "&shareId=" + args[1]            //小程序页面路径；对于小游戏，可以只传入 query 部分，来实现传参效果，如：传入 "?foo=bar"
                };

                WXMediaMessage msg = new WXMediaMessage(miniProgramObj)
                {
                    Title = args[2] + "-美而好，让生活更美好。",                    // 小程序消息title
                    Description = "这是美而好的简介"               // 小程序消息desc
                };

                Stream s = Resources.OpenRawResource(Resource.Drawable.logo);
                Bitmap mBitmap = BitmapFactory.DecodeStream(s);
                msg.SetThumbImage(mBitmap);

                SendMessageToWX.Req req = new SendMessageToWX.Req
                {
                    Transaction = DateTime.Now.ToFileTimeUtc().ToString(),
                    Message = msg,
                    Scene = SendMessageToWX.Req.WXSceneSession  // 目前只支持会话
                };

                wxApi.SendReq(req);

                //br.Dispose();
                //s.Dispose();
            });

            //分享文字给朋友
            MessagingCenter.Subscribe<object, string>(this, "ShareToFriend", (sender, arg) =>
            {
                WXTextObject textObj = new WXTextObject { Text = arg };//定义wx文本对象
                WXMediaMessage msg = new WXMediaMessage { MyMediaObject = textObj, Description = arg };//定义wxmsg对象
                SendMessageToWX.Req req = new SendMessageToWX.Req()
                {
                    Transaction = DateTime.Now.ToFileTimeUtc().ToString(),
                    Message = msg,
                    Scene = SendMessageToWX.Req.WXSceneSession//分享到对话
                };
                wxApi.SendReq(req);
            });
            //分享文字到朋友圈
            MessagingCenter.Subscribe<object, string>(this, "ShareToTimeline", (sender, arg) =>
            {
                WXTextObject textObj = new WXTextObject { Text = arg };//定义wx文本对象
                WXMediaMessage msg = new WXMediaMessage { MyMediaObject = textObj, Description = arg };//定义wxmsg对象
                SendMessageToWX.Req req = new SendMessageToWX.Req()
                {
                    Transaction = DateTime.Now.ToFileTimeUtc().ToString(),
                    Message = msg,
                    Scene = SendMessageToWX.Req.WXSceneTimeline//分享到朋友圈
                };
                wxApi.SendReq(req);
            });
            /*
            //分享网页给朋友
            MessagingCenter.Subscribe<object, string>(this, "ShareToFriend", (sender, arg) =>
            {
                //初始化一个WXWebpageObject，填写url
                WXWebpageObject webpage = new WXWebpageObject();
                webpage.WebpageUrl = "网页url";

                //用 WXWebpageObject 对象初始化一个 WXMediaMessage 对象
                WXMediaMessage msg = new WXMediaMessage(webpage);
                msg.Title = "网页标题 ";
                msg.Description = "网页描述";
                Bitmap thumbBmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
                msg.ThumbData = Util.bmpToByteArray(thumbBmp, true);

                //构造一个Req
                SendMessageToWX.Req req = new SendMessageToWX.Req();
                req.Transaction = buildTransaction("webpage");
                req.Message = msg;
                req.Scene = mTargetScene;
                req.UserOpenId = getOpenId();

                //调用api接口，发送数据到微信
                wxApi.sendReq(req);
            });*/

            base.OnCreate(savedInstanceState);
            Instance = this;
            Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        /*
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }*/

        #region 支付宝
        private void Pay(object sign)
        {
            try
            {
                PayTask payTask = new PayTask(this);
                var result = payTask.PayV2(sign.ToString(), true);
                string Status = result["resultStatus"];

                RunOnUiThread(() => { MessagingCenter.Send(new object(), "PaySuccess", Status); });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 微信
        private bool RegToWx()
        {
            wxApi = WXAPIFactory.CreateWXAPI(this, appID, true);
            return wxApi.RegisterApp(appID);
        }
        #endregion

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                PopupNavigation.Instance.PopAllAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
    }
}

