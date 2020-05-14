using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.Services;
using XMart.Util;
using XMart.Views;

namespace XMart.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private string searchString;   //Comment
        public string SearchString
        {
            get { return searchString; }
            set { SetProperty(ref searchString, value); }
        }

        private ObservableCollection<string> searchHistoryList;   //Comment
        public ObservableCollection<string> SearchHistoryList
        {
            get { return searchHistoryList; }
            set { SetProperty(ref searchHistoryList, value); }
        }

        LocalDatabaseService localDatabaseService = new LocalDatabaseService();

        public Command<string> SearchCommand { get; set; }
        public Command ClearCommand { get; set; }

        public SearchViewModel()
        {
            SearchHistoryList = new ObservableCollection<string>();

            //InitSearchPage();

            SearchCommand = new Command<string>(async (str) =>
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                if (string.IsNullOrEmpty(str))
                {
                    CrossToastPopUp.Current.ShowToastWarning("请输入关键词", ToastLength.Short);
                }
                else
                {
                    if (!SearchHistoryList.Contains(str))
                    {
                        SearchedItem searchedItem = new SearchedItem
                        {
                            createTime = DateTime.UtcNow.ToString(),
                            searchedString = str
                        };
                        await LocalDatabaseHelper<SearchedItem>.InsertOrReplaceAsync(searchedItem);
                    }

                    ProductListPage productListPage = new ProductListPage(str);
                    SearchString = "";
                    await Application.Current.MainPage.Navigation.PushAsync(productListPage);
                }
            }, (str) => { return true; });

            ClearCommand = new Command(async () =>
            {
                bool action = await Application.Current.MainPage.DisplayAlert("确认", "删除所有搜索历史？", "确认", "取消");
                if (action)
                {
                    await localDatabaseService.DeleteAllSearchedItem();
                    InitSearchPage();
                }
            }, () => { return true; });

        }

        public async void InitSearchPage()
        {
            SearchHistoryList.Clear();
            List<SearchedItem> list = await localDatabaseService.GetAllSearchedItem();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    SearchHistoryList.Add(item.searchedString);
                }
            }
        }
    }
}
