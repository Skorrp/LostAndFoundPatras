using LostAndFoundPatras.Models;
using LostAndFoundPatras.ViewModels;
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
    public partial class FoundPetsPage : ContentPage
    {
        public FoundPetsPage()
        {
            InitializeComponent();
            this.BindingContext = new FoundPetListPageViewModel();
        }
    }
}