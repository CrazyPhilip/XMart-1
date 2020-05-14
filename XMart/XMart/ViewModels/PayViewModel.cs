using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.Services;

namespace XMart.ViewModels
{
    public class PayViewModel : BaseViewModel
    {
        private OrderDetail Order { get; set; }

        public Command<string> PayCommand { get; set; }

        //RestSharpService _restSharpService = new RestSharpService();

        public PayViewModel(OrderDetail orderDetail)
        {
            Order = orderDetail;

            PayCommand = new Command<string>((p) =>
            {
                switch (p)
                {
                    case "0":
                        break;
                    case "1":
                        Alipay();
                        break;
                    default:
                        break;
                }
            }, (p) => { return true; });

        }

        /// <summary>
        /// 支付宝
        /// </summary>
        private void Alipay()
        {
            if (Order.orderStatus == "0")
            {
                string body = "test_body";
                foreach (var item in Order.goodsList)
                {
                    body += item.productName + "//";
                }

                //string out_trade_no = "DJ" + DateTime.Now.ToString("yyyyMMddhhmmss");
                string out_trade_no = Order.orderId.ToString();
                string product_code = "QUICK_MSECURITY_PAY";
                string subject = "美而好家具";
                string total_amount = Order.orderTotal.ToString("0.00");

                JObject json = new JObject();
                json.Add("body", body);
                json.Add("out_trade_no", out_trade_no);
                json.Add("product_code", product_code);
                json.Add("subject", subject);
                json.Add("total_amount", total_amount);

                string con = RestSharpService.GetAliPaySign(json.ToString());
                JObject result = JObject.Parse(con);
                string sign = result["data"].ToString();

                MessagingCenter.Send(new object(), "Pay", sign);

                //CrossToastPopUp.Current.ShowToastMessage("支付成功", ToastLength.Long);
                //PayWebPage payWebPage = new PayWebPage();
                //Application.Current.MainPage.Navigation.PushAsync(payWebPage);
            }
        }

    }
}
