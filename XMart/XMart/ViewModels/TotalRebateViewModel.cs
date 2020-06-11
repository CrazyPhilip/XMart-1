using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XMart.Models;
using XMart.Services;
using XMart.Util;
using XMart.ResponseData;
using Newtonsoft.Json;

namespace XMart.ViewModels
{
    public class TotalRebateViewModel : BaseViewModel
    {
		private string totalRebate;   //Comment
		public string TotalRebate
		{
			get { return totalRebate; }
			set { SetProperty(ref totalRebate, value); }
		}

		private ObservableCollection<RebateInfo> rebateList;   //Comment
		public ObservableCollection<RebateInfo> RebateList
		{
			get { return rebateList; }
			set { SetProperty(ref rebateList, value); }
		}

		public TotalRebateViewModel()
		{
            InitAsync();

            /*
            RebateList = new ObservableCollection<RebateInfo>
			{
				new RebateInfo(){ buyerName = "小明", productName = "三叶家私超长沙发", rebate = 267.67},
				new RebateInfo(){ buyerName = "小红", productName = "四叶家私双人床", rebate = 115.35},
				new RebateInfo(){ buyerName = "小强", productName = "五叶家私办公桌", rebate = 195.44},
			};

			double total = 0;
			foreach (var item in RebateList)
			{
				total += item.rebate;
			}
			TotalRebate = total.ToString();*/
		}

		private async System.Threading.Tasks.Task InitAsync()
		{
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                double total = 0;
                string result = await RestSharpHelper<string>.GetAsyncWithoutDeserialization("/member/getAllRebate?designerId=" + GlobalVariables.LoggedUser.id);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    RebateRD rebateRD = JsonConvert.DeserializeObject<RebateRD>(result);

                    if ( rebateRD.result == null || rebateRD.result.Count == 0)
                    {
                        TotalRebate = "0";
                    }
                    else
                    {
                        foreach (var item in rebateRD.result)
                        {
                            total += item.rebate;
                            RebateList.Add(item);
                        }
                        TotalRebate = total.ToString();
                    }
                }
                else
                {
                    TotalRebate = "0";
                    CrossToastPopUp.Current.ShowToastError("Error", ToastLength.Long);
                }

            }
            catch (Exception ex)
            {
                CrossToastPopUp.Current.ShowToastError(ex.Message, ToastLength.Long);
            }
        }
	}
}
