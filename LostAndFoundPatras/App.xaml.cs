using LostAndFoundPatras.Services.Implementations;
using LostAndFoundPatras.Services.Interfaces;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LostAndFoundPatras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            DependencyService.Register<IPetService, PetService>();
            if (Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
            {
                var isUserLoggedIn = Application.Current.Properties["IsUserLoggedIn"];
            }
        }

        protected override void OnStart()
        {
            // set default login on launch
            string IsLogedIn = "False";
            MessagingCenter.Send("this", "S.A.G.A.P.O", IsLogedIn.ToString());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            if (Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
            {
                var isUserLoggedIn = Application.Current.Properties["IsUserLoggedIn"];
                //if (isUserLoggedIn is bool)
                //{
                //    bool xxxxx = (bool)isUserLoggedIn;
                //    if (xxxxx)
                //    {
                //    }
                //}
            }
        }
    }
}
