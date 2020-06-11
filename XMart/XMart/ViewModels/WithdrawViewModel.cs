using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XMart.Models;

namespace XMart.ViewModels
{
    public class WithdrawViewModel : BaseViewModel
    {
        private string withdraw;   //Comment
        public string Withdraw
        {
            get { return withdraw; }
            set { SetProperty(ref withdraw, value); }
        }

        private ObservableCollection<RebateInfo> withdrawList;   //Comment
        public ObservableCollection<RebateInfo> WithdrawList
        {
            get { return withdrawList; }
            set { SetProperty(ref withdrawList, value); }
        }

        public WithdrawViewModel()
        {
            Withdraw = "0";
        }
    }
}
