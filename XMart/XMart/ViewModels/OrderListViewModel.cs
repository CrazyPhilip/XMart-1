﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XMart.Models;
using XMart.ResponseData;
using XMart.Services;
using XMart.Util;
using XMart.Views;

namespace XMart.ViewModels
{
    public class OrderListViewModel : BaseViewModel
    {
		private List<OrderDetail> orderList;   //Comment
		public List<OrderDetail> OrderList
		{
			get { return orderList; }
			set { SetProperty(ref orderList, value); }
		}

		private bool visible;   //Comment
		public bool Visible
		{
			get { return visible; }
			set { SetProperty(ref visible, value); }
		}

		public Command<OrderDetail> EditCommand { get; set; }

		public OrderListViewModel()
		{
			EditCommand = new Command<OrderDetail>((orderDetail) =>
			{
				OrderDetailPage orderDetailPage = new OrderDetailPage(orderDetail);
				Application.Current.MainPage.Navigation.PushModalAsync(orderDetailPage);
			}, (orderDetail) => { return true; });

			InitOrderList();
		}

		private async void InitOrderList()
		{
			try
			{
				RestService _restService = new RestService();
				int userId = GlobalVariables.LoggedUser.id;
				int page = 1;
				int size = 10;
				OrderListRD orderListRD = await _restService.GetOrderListById(userId, page, size);

				if (orderListRD.result.data.Count != 0)
				{
					OrderList = orderListRD.result.data;
					Visible = false;
				}
				else
				{
					Visible = true;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}