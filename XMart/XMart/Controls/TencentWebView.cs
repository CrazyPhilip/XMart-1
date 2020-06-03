using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XMart.Controls
{
    public class TencentWebView : View
    {
        public static readonly BindableProperty UrlProperty = BindableProperty.Create(
            propertyName: "Url",
            returnType: typeof(string),
            declaringType: typeof(TencentWebView),
            defaultValue: default(string));

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

    }
}
