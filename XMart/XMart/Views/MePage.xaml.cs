﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;
using XMart.Util;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MePage : ContentPage
	{
        MePageViewModel mePageViewModel = new MePageViewModel();

		public MePage ()
		{
			InitializeComponent ();

            BindingContext = mePageViewModel;

            //if (GlobalVariables.LoggedUser.userType == "0")
            //{
            //    stack.Children.Add(new CustomView());
            //}
            //else
            //{
            //    stack.Children.Add(new DesignerView());
            //}
		}

        private void TrackVC_Tapped(object sender, EventArgs e)
        {

        }

        private void EvaluationVC_Tapped(object sender, EventArgs e)
        {

        }

        private void AdressManageVC_Tapped(object sender, EventArgs e)
        {

        }

        private void MememberVC_Tapped(object sender, EventArgs e)
        {

        }

        private void ServiceVC_Tapped(object sender, EventArgs e)
        {

        }

        private void SettingVC_Tapped(object sender, EventArgs e)
        {

        }
    }
}