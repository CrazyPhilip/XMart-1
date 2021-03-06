﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XMart.Models;
using XMart.ResponseData;
using XMart.Util;

namespace XMart.Services
{
    public static class RestSharpService
    {
        //private static readonly Lazy<RestSharpService> lazy = new Lazy<RestSharpService>(() => new RestSharpService());
        //
        //public static RestSharpService Instance { get { return lazy.Value; } }
        //
        //private RestSharpService() 
        //{ 
        //
        //}

        #region 会员注册登录
        /// <summary>
        /// 发送验证码 ok
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <returns></returns>
        public static async Task<SimpleRD> SendAuthCode(string tel)
        {
            try
            {
                string url = "/member/getAuthCode?tel=" + tel;

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.GetAsync(url);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> CheckAuthCode(string tel, string authCode)
        {
            try
            {
                string url = "/member/checkAuthCode?tel=" + tel + "&authcode=" + authCode;

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.GetAsync(url);
                return simpleRD;

            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册 ok
        /// </summary>
        /// <param name="registerPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> Register(RegisterPara registerPara)
        {
            try
            {
                string url = "/member/registerByInvite";
                var json = JsonConvert.SerializeObject(registerPara);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// openid注册
        /// </summary>
        /// <param name="registerPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> RegisterByOpenId(RegisterByOpenIdPara registerByOpenIdPara)
        {
            try
            {
                string url = "/member/registerByOpenId";
                var json = JsonConvert.SerializeObject(registerByOpenIdPara);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// openid登录
        /// </summary>
        /// <returns></returns>
        public static async Task<LoginRD> LoginByOpenId(string openId)
        {
            try
            {
                string url = "/member/loginByOpenId?openId=" + openId;

                LoginRD loginRD = await RestSharpHelper<LoginRD>.GetAsync(url);
                return loginRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 忘记密码、重置密码
        /// </summary>
        /// <param name="resetPwdPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> ResetPwd(ResetPwdPara resetPwdPara)
        {
            try
            {
                string url = "/User/resetPwd";
                var json = JsonConvert.SerializeObject(resetPwdPara);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 登录 ok
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static async Task<LoginRD> Login(LoginPara loginPara)
        {
            try
            {
                string url = "/member/loginByTel";
                var json = JsonConvert.SerializeObject(loginPara);

                LoginRD loginRD = await RestSharpHelper<LoginRD>.PostAsync(url, json);
                return loginRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="loginPara"></param>
        /// <returns></returns>
        public static async Task<LoginRD> LoginByAuthCode(LoginPara loginPara)
        {
            try
            {
                string url = "/member/loginByAuthCode";
                var json = JsonConvert.SerializeObject(loginPara);

                LoginRD loginRD = await RestSharpHelper<LoginRD>.PostAsync(url, json);
                return loginRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static async Task<CustomerListRD> GetCustomers(string tel)
        {
            try
            {
                string url = "/member/getClient?phone=" + tel;

                CustomerListRD customerListRD = await RestSharpHelper<CustomerListRD>.GetAsync(url);
                return customerListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="updateUserPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> UpdateUserInfo(UpdateUserPara updateUserPara)
        {
            try
            {
                string url = "/member/updateInfo";
                var json = JsonConvert.SerializeObject(updateUserPara);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改用户绑定微信号
        /// </summary>
        /// <param name="updateUserPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> UpdateWechat(RegisterByOpenIdPara registerByOpenIdPara)
        {
            try
            {
                string url = "/member/updateOpenId";
                var json = JsonConvert.SerializeObject(registerByOpenIdPara);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static async Task<LoginRD> GetUserInfo()
        {
            try
            {
                string url = "/member/getMemberInfo?id=" + GlobalVariables.LoggedUser.id;

                LoginRD loginRD = await RestSharpHelper<LoginRD>.GetAsync(url);
                return loginRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="updateUserPara"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> UploadImage(string imgData)
        {
            try
            {
                string url = "/member/imageUpload";
                var json = "{\"imgData\":\"" + imgData + "\",\"userId\":\"" + GlobalVariables.LoggedUser.id + "\"}";

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public static async Task<RebateRD> GetAllRebate(string designerId)
        {
            try
            {
                string url = "/member/getAllRebate?designerId=" + designerId;

                RebateRD rebateRD = await RestSharpHelper<RebateRD>.GetAsync(url);
                return rebateRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 购物车
        /// <summary>
        /// 获取购物车商品列表
        /// </summary>
        /// <param name="memberId">用户Id</param>
        /// <returns></returns>
        public static async Task<CartItemListRD> GetCartItemList(string memberId)
        {
            try
            {
                string url = "/member/cartList";
                string json = "{\"userId\":" + memberId + "}";

                CartItemListRD cartItemListRD = await RestSharpHelper<CartItemListRD>.PostAsync(url, json);
                return cartItemListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public static async Task<SimpleRD> AddToCart(string memberId, string productId, string num, string attributeValue)
        {
            try
            {
                string url = "/member/addCart";
                string json = "{\"userId\":" + memberId + ", \"productId\":" + productId + ", \"productNum\":" + num + ", \"attributesValues\":\"" + attributeValue + "\"}";

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);

                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public static async Task<StupidRD> DeleteItemInCart(CartItemInfo cartItemInfo)
        {
            try
            {
                string url = "/member/cartDel";
                string json = "{\"userId\":" + GlobalVariables.LoggedUser.id
                    + ",\"checked\":\"" + cartItemInfo.Checked
                    + "\",\"productId\":" + cartItemInfo.productId
                    + ",\"attributesValues\":\"" + cartItemInfo.attributesValues
                    + "\"}";

                StupidRD stupidRD = await RestSharpHelper<StupidRD>.PostAsync(url, json);
                return stupidRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 收货地址
        /// <summary>
        /// 获取收货地址列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static async Task<AddressRD> GetAddressListById(string memberId)
        {
            try
            {
                string url = "/member/addressList";
                string json = "{\"userId\":" + memberId + "}";

                AddressRD addressRD = await RestSharpHelper<AddressRD>.PostAsync(url, json);
                return addressRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加收货地址
        /// </summary>
        /// <param name="addressInfo"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> AddAddress(AddressInfo addressInfo)
        {
            try
            {
                string url = "/member/addAddress";
                string json = JsonConvert.SerializeObject(addressInfo);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改收获地址
        /// </summary>
        /// <param name="addressInfo"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> UpdateAddress(AddressInfo addressInfo)
        {
            try
            {
                string url = "/member/updateAddress";
                string json = JsonConvert.SerializeObject(addressInfo);

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="addressInfo"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> DeleteAddressById(long addressId)
        {
            try
            {
                string url = "/member/delAddress";
                string json = "{\"addressId\":" + addressId + "}";

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 订单
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderPara"></param>
        /// <returns></returns>
        public static StupidRD Order(OrderPara orderPara)
        {
            try
            {
                string url = "/member/addOrder";
                string json = JsonConvert.SerializeObject(orderPara);  //序列化

                StupidRD stupidRD = RestSharpHelper<StupidRD>.Post(url, json);
                return stupidRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据订单编号获取订单详情信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static OrderDetailRD GetOrderDetailByOrderId(long orderId)
        {
            try
            {
                string url = "/member/orderDetail?orderId=" + orderId.ToString();

                OrderDetailRD orderDetailRD = RestSharpHelper<OrderDetailRD>.Get(url);
                return orderDetailRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据用户id获取订单列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static async Task<OrderListRD> GetOrderListById(int userId, int page, int size)
        {
            try
            {
                string url = "/member/orderList?userId=" + userId.ToString()
                    + "&page=" + page.ToString()
                    + "&size=" + size.ToString();

                OrderListRD orderListRD = await RestSharpHelper<OrderListRD>.GetAsync(url);
                return orderListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public static async Task<SimpleRD> CancelOrder(OrderDetail orderDetail)
        {
            try
            {
                string url = "/member/cancelOrder";
                string json = JsonConvert.SerializeObject(orderDetail);  //序列化

                SimpleRD simpleRD = await RestSharpHelper<SimpleRD>.PostAsync(url, json);
                return simpleRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static async Task<StupidRD> DeleteOrder(string orderId)
        {
            try
            {
                string url = "/member/delOrder?orderId=" + orderId;

                StupidRD stupidRD = await RestSharpHelper<StupidRD>.GetAsync(url);
                return stupidRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 商品
        /// <summary>
        /// 获取首页相关内容信息
        /// </summary>
        /// <returns></returns>
        public static async Task<HomeContentRD> GetHomeContent()
        {
            try
            {
                string url = "/goods/home";

                HomeContentRD homeContentRD = await RestSharpHelper<HomeContentRD>.GetAsync(url);
                //HomeContentRD homeContentRD = RestSharpHelper<HomeContentRD>.Get(url);
                return homeContentRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        public static async Task<CategoryRD> GetCategories()
        {
            try
            {
                string url = "/goods/SearchAllItemCat";

                CategoryRD categoryRD = await RestSharpHelper<CategoryRD>.GetAsync(url);
                return categoryRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="sort"></param>
        /// <param name="cid"></param>
        /// <param name="priceGt"></param>
        /// <param name="priceLte"></param>
        /// <returns></returns>
        public static async Task<ProductListRD> GetProductList(int page, int size, string sort, long cid, int priceGt, int priceLte)
        {
            try
            {
                string url = "/goods/allGoods?page=" + page.ToString()
                    + "&size=" + size.ToString()
                    + "&sort=" + sort
                    + "&cid=" + cid.ToString()
                    + "&priceGt=" + priceGt.ToString()
                    + "&priceLte=" + priceLte.ToString();

                ProductListRD productListRD = await RestSharpHelper<ProductListRD>.GetAsync(url);
                return productListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static async Task<ProductDetailRD> GetProductDetail(string productId)
        {
            try
            {
                string url = "/goods/productDet?productId=" + productId;
                if (GlobalVariables.IsLogged)
                {
                    url += "&userId=" + GlobalVariables.LoggedUser.id;
                }
                ProductDetailRD productDetailRD = await RestSharpHelper<ProductDetailRD>.GetAsync(url);
                return productDetailRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 模糊搜索
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sequence"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="sort"></param>
        /// <param name="priceGt"></param>
        /// <param name="priceLte"></param>
        /// <returns></returns>
        public static async Task<ProductListRD> FuzzySearch(string index, int sequence, int page, int size, string sort, int priceGt, int priceLte)
        {
            try
            {
                string url = "/goods/SearchLike?page=" + page.ToString()
                    + "&index=" + index
                    + "&size=" + size.ToString()
                    + "&sort=" + sort
                    + "&sequence=" + sequence.ToString()
                    + "&priceGt=" + priceGt.ToString()
                    + "&priceLte=" + priceLte.ToString();

                ProductListRD productListRD = await RestSharpHelper<ProductListRD>.GetAsync(url);
                return productListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static async Task<StupidRD> AddToCollection(string productId)
        {
            try
            {
                string url = string.Format("/goods/AddCollection?userId={0}&productId={1}", GlobalVariables.LoggedUser.id, productId);

                StupidRD stupidRD = await RestSharpHelper<StupidRD>.GetAsync(url);
                return stupidRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取收藏商品列表
        /// </summary>
        /// <returns></returns>
        public static async Task<ProductListRD> GetCollections()
        {
            try
            {
                string url = "/goods/GetCollection?userId=" + GlobalVariables.LoggedUser.id;

                ProductListRD productListRD = await RestSharpHelper<ProductListRD>.GetAsync(url);
                return productListRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取消收藏商品
        /// </summary>
        /// <returns></returns>
        public static async Task<StupidRD> DeleteCollection(string productId)
        {
            try
            {
                string url = string.Format("/goods/deleteCollection?userId={0}&productId={1}", GlobalVariables.LoggedUser.id, productId);

                StupidRD stupidRD = await RestSharpHelper<StupidRD>.GetAsync(url);
                return stupidRD;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断某商品是否被收藏
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static async Task<string> JudgeCollection(string productId)
        {
            try
            {
                string url = string.Format("/goods/judgeCollection?userId={0}&productId={1}", GlobalVariables.LoggedUser.id, productId);

                string str = await RestSharpHelper<string>.GetAsyncWithoutDeserialization(url);
                return str;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 支付宝
        public static string GetAliPaySign(string json)
        {
            try
            {
                string url = "/AlipyController/appAliPay";

                string str = RestSharpHelper<string>.PostWithoutDeserialization(url, json);
                return str;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 微信
        public static string GetWechatAccessToken(string appId, string secret, string code)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, secret, code);

                string result = RestSharpHelper<string>.GetWithoutDeserialization(url);
                return result;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public static string GetWechatUserInfo(string appId, string secret, string code)
        {
            try
            {
                string url1 = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, secret, code);
                string result1 = RestSharpHelper<string>.GetWithoutDeserialization(url1);
                JObject jObject = JObject.Parse(result1);

                string url2 = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", jObject["access_token"], jObject["openid"]);
                string result2 = RestSharpHelper<string>.GetWithoutDeserialization(url2);
                return result2;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检查更新
        public static async Task<string> GetNewestVersion()
        {
            try
            {
                string url = "/App/getNowApp";

                string str = await RestSharpHelper<string>.GetAsyncWithoutDeserialization(url);
                return str;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
