using Plugin.ContactService.Shared;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XMart.Util;

namespace XMart.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        private ObservableCollection<Contact> contactList;   //Comment
        public ObservableCollection<Contact> ContactList
        {
            get { return contactList; }
            set { SetProperty(ref contactList, value); }
        }

        public Command<string> CallCommand { get; set; }
        public Command<string> EmailCommand { get; set; }
        public Command<string> SendMessageCommand { get; set; }

        public ContactViewModel()
        {
            InitContactPageAsync();

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

        private async Task InitContactPageAsync()
        {
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();

            ContactList = new ObservableCollection<Contact>(contacts);
        }
    }
}
