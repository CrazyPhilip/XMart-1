using System;
using System.Collections.Generic;
using System.Text;
using XMart.Models;
using XMart.Services;
using XMart.Util;
using XMart.ResponseData;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Xamarin.Forms;
using XMart.Views;
using System.Collections.ObjectModel;

namespace XMart.ViewModels
{
    public class AddressManageViewModel : BaseViewModel
    {
        private ObservableCollection<AddressInfo> addressList;   //Comment
        public ObservableCollection<AddressInfo> AddressList
        {
            get { return addressList; }
            set { SetProperty(ref addressList, value); }
        }

        private bool isRefreshing;   //Comment
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        private bool visible;   //Comment
        public bool Visible
        {
            get { return visible; }
            set { SetProperty(ref visible, value); }
        }

        public Command<AddressInfo> EditCommand { get; set; }
        public Command<AddressInfo> DeleteCommand { get; set; }
        public Command<AddressInfo> SetDefaultCommand { get; set; }
        public Command AddAddressCommand { get; set; }
        public Command RefreshCommand { get; set; }

        //RestSharpService _restSharpService = new RestSharpService();

        public AddressManageViewModel()
        {
            AddressList = new ObservableCollection<AddressInfo>();

            EditCommand = new Command<AddressInfo>((address) =>
            {
                //AddressInfo addressInfo = new AddressInfo();
                //
                //foreach (var item in AddressList)
                //{
                //    if (item.addressId == address.addressId)
                //    {
                //        addressInfo = item;
                //        break;
                //    }
                //}

                EditAddressPage editAddressPage = new EditAddressPage(address);
                Application.Current.MainPage.Navigation.PushAsync(editAddressPage);
            }, (id) => { return true; });

            DeleteCommand = new Command<AddressInfo>(async (address) =>
            {
                try
                {
                    SimpleRD simpleRD = await RestSharpService.DeleteAddressById(address.addressId);

                    if (simpleRD.success)
                    {
                        CrossToastPopUp.Current.ShowToastSuccess("删除成功！", ToastLength.Long);
                        InitAddressList();
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError(simpleRD.message, ToastLength.Long);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, (address) => { return true; });

            SetDefaultCommand = new Command<AddressInfo>(async (address) =>
            {
                try
                {
                    if (!Tools.IsNetConnective())
                    {
                        CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                        return;
                    }

                    address.isDefault = true;

                    SimpleRD simpleRD = await RestSharpService.UpdateAddress(address);

                    if (simpleRD.success)
                    {
                        CrossToastPopUp.Current.ShowToastSuccess("更新默认收货地址！", ToastLength.Long);
                        InitAddressList();
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError(simpleRD.message, ToastLength.Long);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, (address) => { return true; });

            AddAddressCommand = new Command(() =>
            {
                EditAddressPage editAddressPage = new EditAddressPage();

                Application.Current.MainPage.Navigation.PushAsync(editAddressPage);
            }, () => { return true; });

            RefreshCommand = new Command(() =>
            {
                InitAddressList();
                IsRefreshing = false;
            }, () => { return true; });

            InitAddressList();
        }

        /// <summary>
        /// 获取地址列表
        /// </summary>
        public async void InitAddressList()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                //RestSharpService _restSharpService = new RestSharpService();

                string memberId = GlobalVariables.LoggedUser.id.ToString();

                AddressRD addressRD = await RestSharpService.GetAddressListById(memberId);

                if (addressRD.result.Count != 0)
                {
                    AddressList = new ObservableCollection<AddressInfo>(addressRD.result);
                    Visible = false;
                }
                else
                {
                    Visible = true;
                    CrossToastPopUp.Current.ShowToastError("无收货地址列表，请添加。", ToastLength.Long);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
