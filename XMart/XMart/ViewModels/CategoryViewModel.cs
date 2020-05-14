using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using XMart.Views;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using System.Collections.ObjectModel;
using XMart.Util;
using XMart.Behaviors;

namespace XMart.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private ObservableCollection<Category> parentCategoryList;   //一级目录
        public ObservableCollection<Category> ParentCategoryList
        {
            get { return parentCategoryList; }
            set { SetProperty(ref parentCategoryList, value); }
        }

        private ObservableCollection<Category> subCategoryList;   //二级目录
        public ObservableCollection<Category> SubCategoryList
        {
            get { return subCategoryList; }
            set { SetProperty(ref subCategoryList, value); }
        }

        private Category selectedParent;   //Comment
        public Category SelectedParent
        {
            get { return selectedParent; }
            set { SetProperty(ref selectedParent, value); }
        }

        public List<Category> categoryList { get; set; }

        public Command SearchCommand { get; set; }
        public Command ReloadCommand { get; set; }
        public Command ParentCategoryTappedCommand { get; set; }
        public Command<int> SubCategoryTappedCommand { get; set; }

        public CategoryViewModel()
        {
            ParentCategoryList = new ObservableCollection<Category>();
            SubCategoryList = new ObservableCollection<Category>();
            categoryList = new List<Category>();

            InitCategories();

            SearchCommand = new Command(() =>
            {
                SearchPage searchPage = new SearchPage();
                Application.Current.MainPage.Navigation.PushAsync(searchPage);

            }, () => { return true; });

            ReloadCommand = new Command(() =>
            {

            }, () => { return true; });

            ParentCategoryTappedCommand = new Command(() =>
            {
                GetSubCategories(SelectedParent.id);
            }, () => { return true; });

            SubCategoryTappedCommand = new Command<int>((id) =>
            {
                foreach (var item in SubCategoryList)
                {
                    if (item.id == id)
                    {
                        ProductListPage productListPage = new ProductListPage(item);
                        Application.Current.MainPage.Navigation.PushAsync(productListPage);
                        break;
                    }
                }

            }, (id) => { return true; });
        }


        /// <summary>
        /// 初始化
        /// </summary>
        private async void InitCategories()
        {
            try
            {
                if (!Tools.IsNetConnective())
                {
                    CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
                    return;
                }

                //RestSharpService _restSharpService = new RestSharpService();
                CategoryRD categoryRD = await RestSharpService.GetCategories();

                categoryList = categoryRD.result;

                foreach (var item in categoryList)
                {
                    if (item.isParent)
                    {
                        ParentCategoryList.Add(item);
                    }
                }

                GetSubCategories(ParentCategoryList[0].id);
                SelectedParent = ParentCategoryList[0];

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取二级目录
        /// </summary>
        /// <param name="index"></param>
        public void GetSubCategories(int parentId)
        {
            SubCategoryList.Clear();

            foreach (var item in categoryList)
            {
                if (item.parentId == parentId)
                {
                    SubCategoryList.Add(item);
                }
            }
        }

    }
}
