using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TotalRebatePage : ContentPage
    {
        TotalRebateViewModel totalRebateViewModel = new TotalRebateViewModel();

        public TotalRebatePage()
        {
            InitializeComponent();

            BindingContext = totalRebateViewModel;
        }
    }
}