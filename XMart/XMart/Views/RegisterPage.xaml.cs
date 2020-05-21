using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using XMart.Util;
using Plugin.Toast;
using Plugin.Toast.Abstractions;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel registerViewModel;

        public RegisterPage()
        {
            InitializeComponent();

            registerViewModel = new RegisterViewModel();

            BindingContext = registerViewModel;
        }

        public RegisterPage(RegisterByOpenIdPara registerByOpenIdPara)
        {
            InitializeComponent();

            registerViewModel = new RegisterViewModel(registerByOpenIdPara);

            BindingContext = registerViewModel;
        }
    }
}