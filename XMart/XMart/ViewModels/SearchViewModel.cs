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

        private ObservableCollection<SearchedItem> searchHistoryList;   //Comment
        public ObservableCollection<SearchedItem> SearchHistoryList
        {
            get { return searchHistoryList; }
            set { SetProperty(ref searchHistoryList, value); }
        }

        LocalDatabase localDatabase = new LocalDatabase();

        public Command SearchCommand { get; set; }

        public SearchViewModel()
        {
            InitSearchPage();

            SearchCommand = new Command(() =>
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                if (string.IsNullOrEmpty(SearchString))
                {
                    CrossToastPopUp.Current.ShowToastWarning("请输入关键词", ToastLength.Short);
                }
                else
                {
                    SearchedItem searchedItem = new SearchedItem
                    {
                        createTime = DateTime.UtcNow.ToString(),
                        searchedString = SearchString
                    };
                    localDatabase.SaveSearchedItem(searchedItem);

                    ProductListPage productListPage = new ProductListPage(SearchString);
                    SearchString = "";
                    Application.Current.MainPage.Navigation.PushModalAsync(productListPage);
                }
            }, () => { return true; });

        }

        private async void InitSearchPage()
        {
            List<SearchedItem> list = await localDatabase.GetAllSearchedItem();
            SearchHistoryList = new ObservableCollection<SearchedItem>(list);
        }
    }
}
