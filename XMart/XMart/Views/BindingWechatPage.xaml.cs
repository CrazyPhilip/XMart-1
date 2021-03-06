﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;
using XMart.Models;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BindingWechatPage : ContentPage
    {
        BindingWechatViewModel bindingWechatViewModel;

        public BindingWechatPage(RegisterByOpenIdPara registerByOpenIdPara)
        {
            InitializeComponent();

            bindingWechatViewModel = new BindingWechatViewModel(registerByOpenIdPara);

            BindingContext = bindingWechatViewModel;
        }

    }
}