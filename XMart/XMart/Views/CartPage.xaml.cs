﻿using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.ViewModels;
using XMart.Services;
using XMart.ResponseData;
using XMart.Util;

namespace XMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        RestService _restService = new RestService();
        CartViewModel cartViewModel = new CartViewModel();

        public CartPage()
        {
            InitializeComponent();

            //InitCart();

            BindingContext = cartViewModel;
        }

        /// <summary>
        /// 初始化购物车
        /// </summary>
        private async void InitCart()
        {
            if (GlobalVariables.IsLogged)
            {
                string memberId = GlobalVariables.LoggedUser.id.ToString();
                CartItemListRD cartItemListRD = await _restService.GetCartItemList(memberId);

                cartViewModel.ItemList = cartItemListRD.result;
                cartViewModel.ItemNumber = cartItemListRD.result.Count().ToString();
            }
        }

        /// <summary>
        /// 单选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SingleCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
            {
                cartViewModel.IsAllChecked = false;
                cartViewModel.AllCheckedButton_Color = Color.LightGray;
            }

            OnCount();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnCount()
        {
            double totalPrice = 0;
            int number = 0;
            foreach (var item in cartViewModel.ItemList)
            {
                if (item._checked == "1")
                {
                    totalPrice += (item.memberPrice * item.productNum);
                    number += item.productNum;
                }
            }

            cartViewModel.TotalSelectedPrice = totalPrice.ToString();
            cartViewModel.CheckedNumber = number;
        }

        private void AllCheckedButton_Clicked(object sender, System.EventArgs e)
        {
            cartViewModel.IsAllChecked = !cartViewModel.IsAllChecked;
            cartViewModel.AllCheckedButton_Color = cartViewModel.IsAllChecked ? Color.Crimson : Color.LightGray;
            foreach (var item in cartViewModel.ItemList)
            {
                //item.IsChecked = cartViewModel.IsAllChecked;
            }

            OnCount();
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            InitCart();
        }
    }
}