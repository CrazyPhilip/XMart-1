using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XMart.Models;
using XMart.Views;

namespace XMart.ViewModels
{
    public class CameraListViewModel : BaseViewModel
    {
        private ObservableCollection<Camera> cameraList;   //Comment
        public ObservableCollection<Camera> CameraList
        {
            get { return cameraList; }
            set { SetProperty(ref cameraList, value); }
        }

        private bool visible;   //Comment
        public bool Visible
        {
            get { return visible; }
            set { SetProperty(ref visible, value); }
        }

        public Command<Camera> SelectCommand { get; set; }

        public CameraListViewModel()
        {
            Visible = false;

            CameraList = new ObservableCollection<Camera>
            {
                new Camera
                {
                    id = "100",
                    direction = "木材加工车间",
                    factoryId = "605",
                    url = "https://open.ys7.com/view/h5/6301918a52f94b3281e5bcb8fe2985f8"
                },
                new Camera
                {
                    id = "101",
                    direction = "包装车间",
                    factoryId = "605",
                    url = "https://open.ys7.com/view/h5/6301918a52f94b3281e5bcb8fe2985f8"
                },
            };

            SelectCommand = new Command<Camera>((camera) =>
            {
                //Launcher.TryOpenAsync(new Uri(camera.url));

                WebPage webPage = new WebPage(camera.url);
                Application.Current.MainPage.Navigation.PushAsync(webPage);
            }, (camera) => { return true; });

        }
    }
}
