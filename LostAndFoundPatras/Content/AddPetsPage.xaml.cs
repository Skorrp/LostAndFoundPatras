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
    }
}