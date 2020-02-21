﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XMart.ResponseData;
using Newtonsoft.Json;
using XMart.Models;
using XMart.Util;

namespace XMart.Services
{
    public class RestService
    {
        HttpClient _client = new HttpClient();
        string rootUrl = "http://118.24.96.42:8082";    //旧
        string rootUrl2 = "http://118.24.96.42:8083";    //新
        string rootUrl3 = "http://120.26.3.153:7777";    //更新
        
        public RestService()
        {

        }
        
        /// <summary>
        /// 获取首页相关内容信息
        /// </summary>
        /// <returns></returns>
        public async Task<HomeContentRD> GetHomeContent()
        {
            string url = rootUrl3 + "/goods/home";

            string responseBody = await GetStringDataAsync(url);

            HomeContentRD homeContentRD = JsonConvert.DeserializeObject<HomeContentRD>(responseBody);

            return homeContentRD;
        }

        /// <summary>
        /// 获取购物车商品列表
        /// </summary>
        /// <param name="memberId">用户Id</param>
        /// <returns></returns>
        public async Task<CartItemListRD> GetCartItemList(string memberId)
        {
            string url = rootUrl + "/member/cartList";
            string httpContent = "userId=" + GlobalVariables.LoggedUser.id;

            string responseBody = await PostAsync(url, httpContent);

            CartItemListRD cartItemListRD = JsonConvert.DeserializeObject<CartItemListRD>(responseBody);

            return cartItemListRD;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<CategoryRD> GetCategories()
        {
            string url = rootUrl3 + "/goods/SearchAllItemCat";

            string responseBody = await GetStringDataAsync(url);

            CategoryRD categoryRD = JsonConvert.DeserializeObject<CategoryRD>(responseBody);

            return categoryRD;
        }

        public async Task<ProductListRD> GetProductList(int page, int size, string sort, long cid, int priceGt, int priceLte)
        {
            string url = rootUrl3 + "/goods/allGoods?page=" + page.ToString()
                + "&size=" + size.ToString()
                + "&sort=" + sort
                + "&cid=" + cid.ToString()
                + "&priceGt=" + priceGt.ToString()
                + "&priceLte=" + priceLte.ToString();

            string responseBody = await GetStringDataAsync(url);

            ProductListRD productListRD = JsonConvert.DeserializeObject<ProductListRD>(responseBody);

            return productListRD;
        }

        /// <summary>
        /// 发送验证码 ok
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <returns></returns>
        public async Task<SimpleRD> SendAuthCode(string tel)
        {
            string url = rootUrl3 + "/member/getAuthCode?tel=" + tel;

            string responseBody = await GetStringDataAsync(url);

            SimpleRD simpleRD = JsonConvert.DeserializeObject<SimpleRD>(responseBody);

            return simpleRD;
        }

        /// <summary>
        /// 注册 ok
        /// </summary>
        /// <param name="registerPara"></param>
        /// <returns></returns>
        public async Task<SimpleRD> Register(RegisterPara registerPara)
        {
            string url = rootUrl3 + "/member/registerByInvite";

            string httpContent = JsonConvert.SerializeObject(registerPara);  //序列化

            string responseBody = await PostAsync(url, httpContent);

            SimpleRD simpleRD = JsonConvert.DeserializeObject<SimpleRD>(responseBody);   //反序列化

            return simpleRD;
        }

        /// <summary>
        /// 忘记密码、重置密码
        /// </summary>
        /// <param name="resetPwdPara"></param>
        /// <returns></returns>
        public async Task<SimpleRD> ResetPwd(ResetPwdPara resetPwdPara)
        {
            string url = rootUrl2 + "/User/resetPwd";

            string httpContent = JsonConvert.SerializeObject(resetPwdPara);

            string responseBody = await PostAsync(url, httpContent);

            SimpleRD simpleRD = JsonConvert.DeserializeObject<SimpleRD>(responseBody);

            return simpleRD;
        }

        /// <summary>
        /// 登录 ok
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public async Task<LoginRD> Login(LoginPara loginPara)
        {
            string url = rootUrl3 + "/member/loginByTel";

            string httpContent = JsonConvert.SerializeObject(loginPara);

            string responseBody = await PostAsync(url, httpContent);

            LoginRD loginRD = JsonConvert.DeserializeObject<LoginRD>(responseBody);

            return loginRD;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductDetailRD> GetProductDetail(string productId)
        {
            string url = rootUrl3 + "/goods/productDet?productId=" + productId;
            string responseBody = await GetStringDataAsync(url);
            ProductDetailRD productDetailRD = JsonConvert.DeserializeObject<ProductDetailRD>(responseBody);
            return productDetailRD;
        }

        #region
        /// <summary>
        /// 截取字符串，处理网站返回值
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        private string GetSubString(string content, string startStr, string endStr)
        {
            int startIndex = content.IndexOf(startStr);
            //int endIndex = content.LastIndexOf(endStr);
            string str = "";

            str = content.Substring(startIndex, content.Length - startIndex);
            str = str.Substring(0, str.LastIndexOf(endStr) + 1);

            return str;
        }

        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<string> GetDataAsync(string uri)
        {
            string responseData = "";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    responseData = GetSubString(content, "{", "}");
                    //responseData = JsonConvert.DeserializeObject<string>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return responseData;
        }

        /// <summary>
        /// Get string方法
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<string> GetStringDataAsync(string uri)
        {
            string responseData = "";
            try
            {
                string response = await _client.GetStringAsync(uri);
                responseData = GetSubString(response, "{", "}");
                //responseData = JsonConvert.DeserializeObject<string>(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return responseData;
        }

        /// <summary>
        /// Post方法
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        private async Task<string> PostAsync(string uri, string httpContent)
        {
            string responseData = "";
            var _httpContent = new StringContent(httpContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync(uri, _httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    responseData = GetSubString(content, "{", "}");
                    //responseData = JsonConvert.DeserializeObject<string>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\tERROR {0}", ex.Message);
            }

            return responseData;
        }

        /// <summary>
        /// Post Form表单的方法
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        private async Task<string> PostFormAsync(string uri, string httpContent)
        {
            string responseData = "";
            var _httpContent = new StringContent(httpContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = await _client.PostAsync(uri, _httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    responseData = GetSubString(content, "{", "}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\tERROR {0}", ex.Message);
            }

            return responseData;
        }
        #endregion
    }
}
