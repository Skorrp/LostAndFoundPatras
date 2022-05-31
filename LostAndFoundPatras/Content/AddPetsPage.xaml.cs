using Firebase.Storage;
using System;
using System.Collections.Generic;
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
        bool isWhite;
        public AddPetsPage()
        {
            InitializeComponent();
        }

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
            RadioButton radioButton = sender as RadioButton;
        }

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
            var downloadlink = await task;
            progressBar.IsVisible = false;
            downloadLink.Text = "Photo uploaded Successfully!";
            //Device.StartTimer(new TimeSpan(0, 0, 3), () =>
            //{
              //  Device.BeginInvokeOnMainThread(() =>
                //{
                  //  downloadLink.Text = "";
                //});
                //return false;
            //});
        }
    }
}