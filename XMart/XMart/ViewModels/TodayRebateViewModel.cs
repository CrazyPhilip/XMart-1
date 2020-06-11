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
    public class TodayRebateViewModel : BaseViewModel
    {
        private string todayRebate;   //Comment
        public string TodayRebate
        {
            get { return todayRebate; }
            set { SetProperty(ref todayRebate, value); }
        }

        private ObservableCollection<RebateInfo> rebateList;   //Comment
        public ObservableCollection<RebateInfo> RebateList
        {
            get { return rebateList; }
            set { SetProperty(ref rebateList, value); }
        }

        public TodayRebateViewModel()
        {
            InitAsync();
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

                    if (rebateRD.result == null || rebateRD.result.Count == 0)
                    {
                        TodayRebate = "0";
                    }
                    else
                    {
                        foreach (var item in rebateRD.result)
                        {
                            DateTime dateTime = DateTime.Parse(item.createDate);
                            if (dateTime.Subtract(DateTime.Now).Days == 0)
                            {
                                total += item.rebate;
                                RebateList.Add(item);
                            }
                        }
                        TodayRebate = total.ToString();
                    }
                }
                else
                {
                    TodayRebate = "0";
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
