using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LostAndFoundPatras.Content;
using System.Diagnostics;

namespace LostAndFoundPatras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContentView : ContentView
    {
        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }
        public HeaderContentView()
        {
            //var instance = new MainPage(); //++++++++++++++++++++++++++++++++++++
            _googleManager = DependencyService.Get<IGoogleManager>();
            InitializeComponent();
            CheckUserLoggedIn();
            StatusCheck();
        }
        private void CheckUserLoggedIn()
        {
            if (!IsLogedIn)
            {
                _googleManager.Login(OnLoginComplete);
            }
        }
        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (IsLogedIn)
            {
                GoogleLogout();
            }
            else
                _googleManager.Login(OnLoginComplete);
        }
        private void GoogleLogout()
        {
            _googleManager.Logout();
            btnLogin.Text = "Login";
            Username.Text = "";
            Email.Text = "";
            imgProfile.Source = "userimage.png";
            StatusCheck();
        }
        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            //MessagingCenter.Send("this", "S.A.G.A.P.O", IsLogedIn.ToString()); // Pass login status to MainPage
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                Username.Text = GoogleUser.Name;
                Email.Text = GoogleUser.Email;
                imgProfile.Source = GoogleUser.Picture;
                StatusCheck();
                btnLogin.Text = "Logout";
                //Application.Current.Properties.Add("IsUserLoggedIn", IsLogedIn);
                //Application.Current.SavePropertiesAsync();
                //Application.Current.Properties["IsUserLoggedIn"] = IsLogedIn.ToString();
            }
        }
        private void StatusCheck()
        {
            if (!Email.Text.Contains("@"))
            {
                IsLogedIn = false;
            }
            else
            {
                IsLogedIn = true;
            }
            MessagingCenter.Send("this", "S.A.G.A.P.O", IsLogedIn.ToString());
        }
    }
}