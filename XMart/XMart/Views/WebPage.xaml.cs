using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : ContentPage
    {
        public WebPage(string url)
        {
            InitializeComponent();

            Web.Source = url;
        }

        public WebPage(HtmlWebViewSource htmlWebViewSource)
        {
            InitializeComponent();

            Web.Source = htmlWebViewSource;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            defaultActivityIndicator.IsRunning = false;    //指示器关闭
            labelLoading.IsVisible = false;
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            defaultActivityIndicator.IsRunning = true;    //指示器开启
            labelLoading.IsVisible = true;
        }

        private void BackButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}