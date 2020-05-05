using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XMart.Controls;
using XMart.Droid;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(XamWebView), typeof(XamWebViewRenderer))]
namespace XMart.Droid
{

    public class XamWebViewRenderer : WebViewRenderer
    {
        static XamWebView _xwebView = null;
        WebView _webView;

        public XamWebViewRenderer(Context context) : base(context)
        {

        }

        class XamWebViewClient : Android.Webkit.WebViewClient
        {
            public override async void OnPageFinished(WebView view, string url)
            {
                if (_xwebView != null)
                {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0)
                        await System.Threading.Tasks.Task.Delay(100);// 这里的时间可以调整
                    _xwebView.HeightRequest = view.ContentHeight;
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            _xwebView = e.NewElement as XamWebView;
            _webView = Control;

            if (e.OldElement == null)
            {
                _webView.SetWebViewClient(new XamWebViewClient());
            }

        }
    }
}