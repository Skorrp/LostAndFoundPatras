using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LostAndFoundPatras.Content;
using System.Diagnostics;
using Plugin.LocalNotifications;
using LostAndFoundPatras.Models;
using System.Threading;

namespace LostAndFoundPatras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContentView : ContentView
    {
        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }
        public string Initializer { get; set; }

        readonly List<PetModel> petlist = new List<PetModel>();
        public HeaderContentView()
        {
            MessagingCenter.Subscribe<PetModel>(this, "LocationData", pet =>
            {
                if (pet.Latitude != null)
                {
                    petlist.Add(pet);
                    petlist.ForEach(pets => Console.WriteLine($"ABCDEFG {pets.Latitude}, {pets.Longitude}"));
                    Console.WriteLine($"Hello {pet.Latitude}, {pet.Longitude}");
                }
            });
                _googleManager = DependencyService.Get<IGoogleManager>();
            InitializeComponent();
            CheckUserLoggedIn();
            StatusCheck();
            try
            {
                DelayActionAsync(10000).Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public async Task DelayActionAsync(int delay)
        {
            await Task.Run(async() => { 
                await Task.Delay(delay);
                Console.WriteLine("THE PETLIST CONTAINS " + petlist.Count + " ITEMS");
            });
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
                //CrossLocalNotifications.Current.Show("Login Details", "You are not logged in!");
            }
            else
            {
                IsLogedIn = true;
                //CrossLocalNotifications.Current.Show("Login Details", "You are now logged in!");
            }
            MessagingCenter.Send("this", "S.A.G.A.P.O", IsLogedIn.ToString());
            Initializer = IsLogedIn.ToString();
        }

        private void switchLocationTracking_Toggled(object sender, ToggledEventArgs e)
        {
            var deviceLocation = new LocationTrackingPage();
            deviceLocation.GomuGomuNo(); // <-- Enables device location tracking
            int i = 0;
            // Receives device location (runs every 2 seconds)
            MessagingCenter.Subscribe<LocationMessage>(this, "Location", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Console.WriteLine($"{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}");
                    foreach (var pet in petlist)
                    {
                        double distance = Distance(Double.Parse(pet.Latitude), Double.Parse(pet.Longitude), message.Latitude, message.Longitude);
                        Console.WriteLine($"Distance is {distance} KM");
                        if (distance < 0.3)
                        {
                            var seconds = TimeSpan.FromSeconds(10);
                            i++;
                            Device.StartTimer(seconds, () =>
                            {
                                Debug.WriteLine($"i:{i}");
                                if (i >= 15)
                                {
                                    CrossLocalNotifications.Current.Show("A pet has gone missing nearby","Click to open the app and help find it!");
                                    i = 0;
                                    return false;
                                }
                                return false;
                            });
                        }
                    }
                });
            });
            if (switchLocationTracking.IsToggled == false)
            {
                MessagingCenter.Unsubscribe<LocationMessage>(this, "Location");
            }

            double Distance (double lat1, double lon1, double lat2, double lon2)
            {
                // Haversine formula
                double dlon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
                double dlat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;

                double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                           Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                           Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                // Radius of earth in
                // kilometers. Use 3956
                // for miles
                double r = 6371.137;
                //Debug.WriteLine($"Device Location is X: {lat2} , Y: {lon2} and the distance between the phone and the lost pet ({lat1} - {lon1} is {Distance(lat1, lon1, lat2, lon2)}");
                // calculate the result
                return (c * r);
            }
        }
    }
}