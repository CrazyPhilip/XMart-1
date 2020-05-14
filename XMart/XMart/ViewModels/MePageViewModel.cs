using System;
using System.Collections.Generic;
using System.Text;
using XMart.Util;
using XMart.Views;
using Xamarin.Forms;
using System.IO;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using XMart.Services;
using XMart.ResponseData;
using System.Collections.ObjectModel;

namespace XMart.ViewModels
{
    public class MePageViewModel : BaseViewModel
    {
		private string userName;   //Comment
		public string UserName
		{
			get { return userName; }
			set { SetProperty(ref userName, value); }
		}

		private string userType;   //Comment
		public string UserType
		{
			get { return userType; }
			set { SetProperty(ref userType, value); }
		}

		private string userId;   //Comment
		public string UserId
		{
			get { return userId; }
			set { SetProperty(ref userId, value); }
		}

		private string userAvatar;   //Comment
		public string UserAvatar
		{
			get { return userAvatar; }
			set { SetProperty(ref userAvatar, value); }
		}

		private bool designerVisible;   //Comment
		public bool DesignerVisible
		{
			get { return designerVisible; }
			set { SetProperty(ref designerVisible, value); }
		}

		private bool customerVisible;   //Comment
		public bool CustomerVisible
		{
			get { return customerVisible; }
			set { SetProperty(ref customerVisible, value); }
		}

		private int orderNumber;   //Comment
		public int OrderNumber
		{
			get { return orderNumber; }
			set { SetProperty(ref orderNumber, value); }
		}

		private int collectionNumber;   //Comment
		public int CollectionNumber
		{
			get { return collectionNumber; }
			set { SetProperty(ref collectionNumber, value); }
		}

		private ObservableCollection<Option> optionList;   //Comment
		public ObservableCollection<Option> OptionList
		{
			get { return optionList; }
			set { SetProperty(ref optionList, value); }
		}

		public Command<string> NavigateCommand { get; set; }
		public Command LoginOutCommand { get; set; }
		public Command ReloadCommand { get; set; }
		public Command EditUserInfoCommand { get; set; }

		public MePageViewModel()
		{


			UserName = string.Empty;
			UserType = string.Empty;
			UserId = string.Empty;
			UserAvatar = string.Empty;
			DesignerVisible = false;
			CustomerVisible = false;

			InitMePage();

			NavigateCommand = new Command<string>((pageName) =>
			{
				if (!string.IsNullOrWhiteSpace(pageName))
				{
					Type type = Type.GetType(pageName);
					Page page = (Page)Activator.CreateInstance(type);
					Application.Current.MainPage.Navigation.PushAsync(page);
				}
			}, (pageName) => { return true; });

			LoginOutCommand = new Command(async () =>
			{
				bool action = await Application.Current.MainPage.DisplayAlert("退出登录", "确定要退出登录吗？", "确定", "取消");
				if (action)
				{
					LoginOut();
				}
			}, () => { return true; });

			ReloadCommand = new Command(() =>
			{
				InitMePage();
			}, () => { return true; });

		}

		public void InitMePage()
		{
			try
			{
				UserName = GlobalVariables.LoggedUser.username;
				UserId = GlobalVariables.LoggedUser.id.ToString();
				UserType = GlobalVariables.LoggedUser.userType == "0" ? "客户" : "设计师";
				UserAvatar = GlobalVariables.LoggedUser.pricture == null ? "star_yellow.png" : GlobalVariables.LoggedUser.pricture;
				CustomerVisible = GlobalVariables.LoggedUser.userType == "0" ? true : false;
				DesignerVisible = !CustomerVisible;

				if (GlobalVariables.LoggedUser.userType == "1")
				{
					OptionList = new ObservableCollection<Option>
					{
						new Option { icon = "money_today.png", option = "今日收益", page = "" },
						new Option { icon = "money_all.png", option = "总收益", page = "" },
						new Option { icon = "money_withdraw.png", option = "可提现", page = "" },
						new Option { icon = "customers.png", option = "我的客户", page = "XMart.Views.CustomerListPage" },
						new Option { icon = "orders.png", option = "我的订单", page = "XMart.Views.OrderListPage" },
						new Option { icon = "star_blue.png", option = "我的收藏", page = "XMart.Views.CollectionPage" },
						new Option { icon = "location.png", option = "地址管理", page = "XMart.Views.AddressManagePage" },
						new Option { icon = "setting.png", option = "系统设置", page = "XMart.Views.SettingPage" }
					};
				}
				else
				{
					OptionList = new ObservableCollection<Option>
					{
						new Option { icon = "orders.png", option = "我的订单", page = "XMart.Views.OrderListPage" },
						new Option { icon = "star_blue.png", option = "我的收藏", page = "XMart.Views.CollectionPage" },
						new Option { icon = "location.png", option = "地址管理", page = "XMart.Views.AddressManagePage" },
						new Option { icon = "setting.png", option = "系统设置", page = "XMart.Views.SettingPage" }
					};
				}

				//if (!Tools.IsNetConnective())
				//{
				//	CrossToastPopUp.Current.ShowToastError("无网络连接，请检查网络。", ToastLength.Long);
				//	return;
				//}
				//
				//RestSharpService _restSharpService = new RestSharpService();
				//OrderListRD orderListRD = await _restSharpService.GetOrderListById(order)
			}
			catch (Exception)
			{
				throw;
			}
		}

		private	void LoginOut()
		{
			GlobalVariables.IsLogged = false;

			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.dat");
			File.Delete(fileName);

			MainPage mainPage = new MainPage();
			Application.Current.MainPage.Navigation.PushAsync(mainPage);
			Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[0]);
		}

		public class Option
		{
			public string option { get; set; }
			public string icon { get; set; }
			public string page { get; set; }
		}
	}
}
