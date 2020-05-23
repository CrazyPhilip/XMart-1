using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;
using XMart.Models;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Extensions;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShareView : PopupPage
    {
        public ShareView(ProductInfo product)
        {
            InitializeComponent();

            BindingContext = new ShareViewModel(product);
        }

    }
}