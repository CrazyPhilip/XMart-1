using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.Util;
using XMart.ViewModels;
using XMart.Services;
using XMart.ResponseData;
using XMart.Models;
using XMart.Behaviors;
using Plugin.Toast;
using Plugin.Toast.Abstractions;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryPage : ContentPage
	{
        CategoryViewModel categoryViewModel;

		public CategoryPage ()
		{
			InitializeComponent ();

            categoryViewModel = new CategoryViewModel();

            BindingContext = categoryViewModel;
        }

    }
}