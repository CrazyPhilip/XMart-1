using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XMart.Models;
using XMart.Util;
using Plugin.Toast;
using Plugin.Toast.Abstractions;

namespace XMart.ViewModels
{
    public class ShareViewModel : BaseViewModel
    {
        public Command<string> ShareCommand { get; set; }

        public ShareViewModel(ProductInfo product)
        {
            ShareCommand = new Command<string>(async (p) =>
            {
                switch (p)
                {
                    case "0":
                        {
                            string para2 = "{\"p\":" + product.productId + ",\"u\":" + GlobalVariables.LoggedUser.id + "}";
                            byte[] byteArray = Encoding.UTF8.GetBytes(para2);
                            string encodedPara = Convert.ToBase64String(byteArray);

                            string sharedString = "復制此鏈接，打開美而好。#" + product.productName + "#" + encodedPara;

                            MessagingCenter.Send(new object(), "Register");//首先进行注册，然后订阅注册的结果。
                            MessagingCenter.Send(new object(), "ShareToFriend", sharedString);
                        }
                        break;

                    case "1":
                        {
                            //string para = "?productId=" + product.productId + "&userId=" + GlobalVariables.LoggedUser.id;
                            //MessagingCenter.Send(new object(), "Register");//首先进行注册，然后订阅注册的结果。
                            //MessagingCenter.Send(new object(), "ShareMiniProgramToFriend", para);
                        }
                        break;

                    case "2":
                        {
                            //string para = "?productId=" + product.productId + "&userId=" + GlobalVariables.LoggedUser.id;
                            //MessagingCenter.Send(new object(), "Register");//首先进行注册，然后订阅注册的结果。
                            //MessagingCenter.Send(new object(), "ShareToTimeline", para);
                        }
                        break;

                    case "3":
                        {
                            string para2 = "{\"p\":" + product.productId + ",\"u\":" + GlobalVariables.LoggedUser.id + "}";
                            byte[] byteArray = Encoding.UTF8.GetBytes(para2);
                            string encodedPara = Convert.ToBase64String(byteArray);

                            string sharedString = "復制此鏈接，打開美而好。#" + product.productName + "#" + encodedPara;

                            await Clipboard.SetTextAsync(sharedString);
                            CrossToastPopUp.Current.ShowToastSuccess("已复制到剪贴板", ToastLength.Short);
                        }
                        break;
                    default:
                        break;
                }
            }, (p) => { return true; });

        }
    }
}
