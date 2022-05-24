using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            _googleManager = DependencyService.Get<IGoogleManager>();
           // CheckUserLoggedIn();
            InitializeComponent();
        }
        //private void CheckUserLoggedIn()
        //{
        //    _googleManager.Login(OnLoginComplete);
        //}
        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            
            if (btnLogin.Text == "Logout")
            {
              GoogleLogout();
            }
            else
                _googleManager.Login(OnLoginComplete);
        }
        void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
            btnLogin.Text = "Login";
            Username.Text = "";
            Email.Text = "";
            imgProfile.Source = "userimage.png";
        }
        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                Username.Text = GoogleUser.Name;
                Email.Text = GoogleUser.Email;
                imgProfile.Source = GoogleUser.Picture;
                IsLogedIn = true;
                btnLogin.Text = "Logout";
            }
        }
    }
}