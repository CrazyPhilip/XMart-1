using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using Com.Tencent.MM.Opensdk.Constants;
using Com.Tencent.MM.Opensdk.Modelbase;
using Com.Tencent.MM.Opensdk.Modelmsg;
using Com.Tencent.MM.Opensdk.Openapi;
using Org.Json;
using Plugin.Toast;
using System;
using Exception = Java.Lang.Exception;
using XMart.Services;
using Xamarin.Forms;

namespace XMart.Droid.wxapi
{
    [Activity(Name = "com.wyhl.XMart.wxapi.WXEntryActivity", Label = "@string/app_name", Exported = true, 
        LaunchMode = Android.Content.PM.LaunchMode.SingleTop, TaskAffinity = "XMart")]
    public class WXEntryActivity : Activity, IWXAPIEventHandler
    {
        private readonly string appID = "wxfad74b8fe74b6c22";//申请的appid
        private readonly string appSecret = "bd6a3cf325e5ac237bc8f788b61a7706";//appSecret
        private MyHandler handler;
        private string TAG = "MyHandler";
        private IWXAPI api;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            api = WXAPIFactory.CreateWXAPI(this, appID, true);
            handler = new MyHandler(this);

            try
            {
                Intent intent = this.Intent;
                bool b = api.HandleIntent(intent, this);
            }
            catch (Exception e)
            {
                Log.Error(TAG, e.StackTrace);
            }
        }

        public new void Dispose()
        {
            this.Dispose();
        }

        protected void onNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            this.Intent = intent;

            api.HandleIntent(intent, this);
        }

        // 微信发送请求到第三方应用时，会回调到该方法
        public void OnReq(BaseReq req)
        {
            switch (req.Type)
            {
                case ConstantsAPI.CommandSendauth:
                    break;
                case ConstantsAPI.CommandGetmessageFromWx:
                    break;
                case ConstantsAPI.CommandShowmessageFromWx:
                    break;
                default:
                    break;
            }
        }

        // 第三方应用发送到微信的请求处理后的响应结果，会回调到该方法
        public void OnResp(BaseResp resp)
        {
            const int RETURN_MSG_TYPE_LOGIN = 1;
            const int RETURN_MSG_TYPE_SHARE = 2;
            
            CrossToastPopUp.Current.ShowToastSuccess("微信回调成功2", Plugin.Toast.Abstractions.ToastLength.Short);

            switch (resp.MyErrCode)
            {
                case BaseResp.ErrCode.ErrAuthDenied:
                    CrossToastPopUp.Current.ShowToastError("用户拒绝授权", Plugin.Toast.Abstractions.ToastLength.Short);
                    Finish();
                    break;

                case BaseResp.ErrCode.ErrUserCancel:
                    if (RETURN_MSG_TYPE_SHARE == resp.Type)
                        CrossToastPopUp.Current.ShowToastError("分享失败", Plugin.Toast.Abstractions.ToastLength.Short);
                    else
                        CrossToastPopUp.Current.ShowToastError("用户取消", Plugin.Toast.Abstractions.ToastLength.Short);
                    Finish();
                    break;

                case BaseResp.ErrCode.ErrOk:
                    switch (resp.Type)
                    {
                        case RETURN_MSG_TYPE_LOGIN:
                            //拿到了微信返回的code,立马再去请求access_token
                            string code = ((SendAuth.Resp)resp).Code;

                            //就在这个地方，用网络库什么的或者自己封的网络api，发请求去咯，注意是get请求
                            string result = RestSharpService.GetWechatUserInfo(appID, appSecret, code);
                            RunOnUiThread(() => { MessagingCenter.Send(new object(), "LoginSuccess", result); });

                            break;

                        case RETURN_MSG_TYPE_SHARE:
                            CrossToastPopUp.Current.ShowToastSuccess("分享", Plugin.Toast.Abstractions.ToastLength.Short);
                            Finish();
                            break;

                    }
                    Finish();
                    break;

                default:
                    Finish();
                    break;
            }
        }

        class MyHandler : Handler
        {
            private string TAG = "MyHandler";
            private WeakReference<WXEntryActivity> wxEntryActivityWeakReference;

            public MyHandler(WXEntryActivity wxEntryActivity)
            {
                wxEntryActivityWeakReference = new WeakReference<WXEntryActivity>(wxEntryActivity);
            }

            public void handleMessage(Message msg)
            {
                //int tag = msg.What;
                Bundle data = msg.Data;
                try
                {
                    JSONObject json = new JSONObject(data.GetString("result"));
                    string openId, accessToken, refreshToken, scope;
                    openId = json.GetString("openid");
                    accessToken = json.GetString("access_token");
                    refreshToken = json.GetString("refresh_token");
                    scope = json.GetString("scope");

                    WXEntryActivity wxentry;
                    wxEntryActivityWeakReference.TryGetTarget(out wxentry);
                    Intent intent = new Intent(wxentry, typeof(WXEntryActivity));
                    intent.PutExtra("openId", openId);
                    intent.PutExtra("accessToken", accessToken);
                    intent.PutExtra("refreshToken", refreshToken);
                    intent.PutExtra("scope", scope);
                    wxentry.StartActivity(intent);
                }
                catch (JSONException ex)
                {
                    Log.Error(TAG, ex.Message);
                }
            }
        }
    }
}