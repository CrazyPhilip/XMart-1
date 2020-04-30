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
    public partial class CameraListPage : ContentPage
    {
        CameraListViewModel cameraListViewModel = new CameraListViewModel();

        public CameraListPage()
        {
            InitializeComponent();

            BindingContext = cameraListViewModel;
        }

        protected override void OnDisappearing()
        {
            list.SelectedItem = -1;
        }
    }
}