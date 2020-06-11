using Android.Content;
using XMart.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XMart.Droid;
using System.ComponentModel;
using Com.Tencent.Smtt.Sdk;

[assembly: ExportRenderer(typeof(TencentWebView), typeof(TencentWebViewRenderer))]
namespace XMart.Droid
{
    public class TencentWebViewRenderer : ViewRenderer<TencentWebView, Android.Widget.RelativeLayout>
    {
        private Android.Widget.RelativeLayout mRelativeLayout;
        private Com.Tencent.Smtt.Sdk.WebView tencentWebView;
        Context _context;

        public TencentWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TencentWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe from event handlers and cleanup any resources
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    // Instantiate the native control and assign it to the Control property with
                    // the SetNativeControl method
                    mRelativeLayout = Inflate(Context, Resource.Layout.WebViewLayout, null) as Android.Widget.RelativeLayout;

                    tencentWebView = mRelativeLayout.FindViewById<Com.Tencent.Smtt.Sdk.WebView>(Resource.Id.forum_context);
                    tencentWebView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                    //x变量非null表示启用x5内核成功
                    var x = tencentWebView.X5WebViewExtension;
                    tencentWebView.SetMinimumWidth(100);
                    tencentWebView.SetMinimumHeight(800);
                    //tencentWebView.SetLayerType();
                    tencentWebView.Settings.UseWideViewPort = true;
                    tencentWebView.Settings.LoadWithOverviewMode = true;
                    tencentWebView.Settings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.SingleColumn);
                    tencentWebView.Settings.TextZoom = 100;
                    tencentWebView.DrawingCacheEnabled = true;

                    SetNativeControl(mRelativeLayout);
                    tencentWebView.LoadUrl(e.NewElement.Url);
                }
                // Configure the control and subscribe to event handlers

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var webView = (TencentWebView)sender;

            if (e.PropertyName == TencentWebView.UrlProperty.PropertyName)
            {
                tencentWebView.LoadUrl(webView.Url);
            }
        }
    }
}