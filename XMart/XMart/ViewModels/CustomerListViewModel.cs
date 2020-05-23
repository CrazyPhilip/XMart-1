using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using XMart.Util;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace XMart.ViewModels
{
    public class CustomerListViewModel : BaseViewModel
    {
        private List<UserInfo> customerList;   //Comment
        public List<UserInfo> CustomerList
        {
            get { return customerList; }
            set { SetProperty(ref customerList, value); }
        }

        private bool visible;   //Comment
        public bool Visible
        {
            get { return visible; }
            set { SetProperty(ref visible, value); }
        }

        public Command<string> CallCommand { get; set; }
        public Command<string> EmailCommand { get; set; }
        public Command<string> SendMessageCommand { get; set; }

        public CustomerListViewModel()
        {
            CustomerList = new List<UserInfo>();

            InitCustomerListAsync();

            CallCommand = new Command<string>((tel) =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(tel))
                    {
                        PhoneDialer.Open(tel);
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("电话号码为空", ToastLength.Long);
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    // Phone Dialer is not supported on this device.
                    CrossToastPopUp.Current.ShowToastError("该设备不支持拨号", ToastLength.Long);
                }
                catch (Exception)
                {
                    // Other error has occurred.
                }
            }, (tel) => { return true; });

            EmailCommand = new Command<string>((email) =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(email) && !Tools.CheckEmail(email))
                    {
                        //var message = new EmailMessage
                        //{
                        //    Subject = subject,
                        //    Body = body,
                        //    To = recipients,
                        //    //Cc = ccRecipients,
                        //    //Bcc = bccRecipients
                        //};
                        //await Email.ComposeAsync(message);
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("电子邮箱格式不正确", ToastLength.Long);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, (email) => { return true; });

            SendMessageCommand = new Command<string>(async (tel) =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(tel))
                    {
                        string result = await Application.Current.MainPage.DisplayPromptAsync("短信", "请输入短消息", "发送", "取消", "短消息（140个字以内）", 140, null);

                        if (result == null)
                        {
                            CrossToastPopUp.Current.ShowToastWarning("已取消", ToastLength.Long);
                        }
                        else if (result == "")
                        {
                            CrossToastPopUp.Current.ShowToastWarning("请输入短消息（140字以内）", ToastLength.Long);
                        }
                        else
                        {
                            var message = new SmsMessage()
                            {
                                Body = result,
                                Recipients = new List<string>() { tel }
                            };
                            await Sms.ComposeAsync(message);
                        }
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("电话号码为空", ToastLength.Long);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }, (tel) => { return true; });
        }

        private async void InitCustomerListAsync()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                //RestSharpService _restSharpService = new RestSharpService();
                CustomerListRD customerListRD = await RestSharpService.GetCustomers(GlobalVariables.LoggedUser.phone.ToString());

                if (customerListRD.result.Count > 0)
                {
                    List<UserInfo> temp = new List<UserInfo>();
                    foreach (var item in customerListRD.result)
                    {
                        temp.Add(item);
                    }
                    CustomerList = temp;

                    Visible = false;
                }
                else
                {
                    Visible = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
