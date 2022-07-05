using Firebase.Storage;
using LostAndFoundPatras.Models;
using LostAndFoundPatras.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LostAndFoundPatras.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPetsPage : ContentPage
    {
        private string downloadlink = string.Empty;
        bool isWhite = false;
        bool okayPhone = false, okayPhoto = false, okayLost = false, okayDescription = false, okayArea = false, okayEmail = false;
        RadioButton radioButton;

        public AddPetsPage()
        {
            InitializeComponent();
            this.BindingContext = new AddPetsPageViewModel();
            BtnDisable();
            MessagingCenter.Subscribe<string, string>("this", "S.A.G.A.P.O", (sender, message) => {
                string data = message.ToString();
                if (data == "True")
                {
                    LoggedInContent.IsVisible = true;
                    NotLoggedInContent.IsVisible = false;
                }
                else
                {
                    LoggedInContent.IsVisible = false;
                    NotLoggedInContent.IsVisible = true;
                }
            });
        }

        public AddPetsPage(PetModel pet)
        {
            InitializeComponent();
            this.BindingContext = new AddPetsPageViewModel(pet);
        }

        //Change to dark theme and vice versa
        private void btnTheme_Clicked(object sender, EventArgs e)
        {
            if (isWhite)
            {
                BackgroundColor = Color.FromRgb(14, 43, 64);
                btnTheme.BackgroundColor = Color.White;
                btnTheme.TextColor = Color.FromRgb(14, 43, 64);
                isWhite = false;
            }
            else
            {
                BackgroundColor = Color.White;
                btnTheme.BackgroundColor = Color.FromRgb(14, 43, 64);
                btnTheme.TextColor = Color.White;
                isWhite = true;
            }
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            radioButton = sender as RadioButton;
            _lost.Text = $"{radioButton.Content}";
        }
        //Upload Photo
        async private void Button_Clicked(object sender, EventArgs e)
        {
            var photo = await Xamarin.Essentials.MediaPicker.PickPhotoAsync();
            if (photo == null)
                return;
            var task = new FirebaseStorage("lafp-58cbb.appspot.com", new FirebaseStorageOptions
            {
                ThrowOnCancel = true
            })
                .Child("petsPhotoFolder")
                .Child(photo.FileName)
                .PutAsync(await photo.OpenReadAsync());

            task.Progress.ProgressChanged += (s, args) =>
            {
                progressBar.IsVisible = true;
                progressBar.Progress = args.Percentage;
            };
            downloadlink = await task;
            progressBar.IsVisible = false;
            //Device.StartTimer(new TimeSpan(0, 0, 3), () =>
            //{
            //  Device.BeginInvokeOnMainThread(() =>
            //{
            //  downloadLink.Text = "";
            //});
            //return false;
            //});
            downloadLink.Text = downloadlink;
            string ddd = PetDate.Date.ToString();
            _date.Text = ddd;

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            radioButton.IsChecked = false;
            btnSubmit.IsEnabled = false;
            downloadlink = "";
            area.Text = "";
            description.Text = "";
        }        

        // Disables submit button on page load
        public void BtnDisable()
        {
            if (email.Text == null)
            {
                btnSubmit.IsEnabled = false;
            }
            else
                btnSubmit.IsEnabled = true;
        }
        public void Format()
        {
            if (email.Text == null)
            {
                email.Text = "";
            }
            if (email.Text.Contains("@") && email.Text.Contains(".") && email.Text.Length > 10)
            {
                okayEmail = true;
            }
            if (phone.Text == null)
            {
                phone.Text = "";
            }
            if (phone.Text.Length == 10)
            {
                okayPhone = true;
            }
            else
            {
                okayPhone = false;
            }
            if (downloadlink != null)
                if (downloadlink.StartsWith("https://firebasestorage."))
                {
                    okayPhoto = true;
                }
            if (this.ProcessSelection() == false)
                App.Current.MainPage.DisplayAlert("Oops!", "Don't forget to select between Lost and Found", "Ok");
            if (description.Text == null)
            {
                description.Text = "";
            }
            if (description.Text.Length > 0)
            {
                okayDescription = true;
            }
            if (area.Text == null)
            {
                area.Text = "";
            }
            if (area.Text.Length > 0)
            {
                okayArea = true;
            }
               // Debug.WriteLine($"Phone:{okayPhone}, Photo:{okayPhoto}, Lost:{okayLost}, Description:{okayDescription}, Area:{okayArea}, Email:{okayEmail}");
            if (okayPhone && okayPhoto && okayLost && okayDescription && okayArea && okayEmail)
            {
                btnSubmit.IsEnabled = true;
            }
            else
            {
                btnSubmit.IsEnabled = false;
            }
        }
        // email validation
        private void email_Unfocused(object sender, FocusEventArgs e)
        {
            Format();
        }
        private bool ProcessSelection()
        {
            bool IsOK = true;
            if (radioButton.IsChecked)
            {
                okayLost = true;
            }
            else
                IsOK = false;
            return IsOK;
        }
    }
}