using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMart.Views;
using XMart.Util;
using XMart.Services;
using System.IO;
using XMart.Controls;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XMart
{
    public partial class App : Xamarin.Forms.Application
    {
        public static double ScreenWidth;
        public static double ScreenHeight;

        //private static LocalDatabaseHelper database;   //Comment
        //public static LocalDatabaseHelper Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            database = new LocalDatabaseHelper(Path.Combine(FileSystem.CacheDirectory, "XMartSQLite.db3"));
        //        }
        //        return database;
        //    }
        //}

        public App()
        {
            InitializeComponent();

            MainPage mainPage = new MainPage();
            MyNavigationPage navigationPage = new MyNavigationPage(mainPage)
            {
                BarBackgroundColor = Color.FromHex("fafafa"),
                BarTextColor = Color.Black
            };

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
