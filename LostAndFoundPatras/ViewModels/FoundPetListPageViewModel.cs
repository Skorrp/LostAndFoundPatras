using LostAndFoundPatras.Models;
using LostAndFoundPatras.Services.Interfaces;
using LostAndFoundPatras.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LostAndFoundPatras.ViewModels
{
    internal class FoundPetListPageViewModel : BaseViewModel
    {
        #region Properties
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private readonly IPetService _petService;

        public ObservableCollection<PetModel> Pets { get; set; } = new ObservableCollection<PetModel>();
        #endregion

        #region Constructor
        public FoundPetListPageViewModel()
        {
            _petService = DependencyService.Resolve<IPetService>();
            GetFoundPets();
        }
        #endregion

        #region Methods
        private void GetFoundPets()
        {
            IsBusy = true;
            Task.Run(async () =>
            {
                var petList = await _petService.GetFoundPets();

                Device.BeginInvokeOnMainThread(() =>
                {

                    Pets.Clear();
                    if (petList?.Count > 0)
                    {
                        foreach (var pet in petList)
                        {
                            Pets.Add(pet);
                        }
                    }
                    IsBusy = IsRefreshing = false;
                });

            });
        }
        #endregion

        #region Commands

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            GetFoundPets();
        });

        public ICommand SelectedPetCommand => new Command<PetModel>(async (pet) =>
        {
            if (pet != null)
            {
                var response = await App.Current.MainPage.DisplayActionSheet("Options", "Cancel", null, "Update Pet", "Delete Pet");

                if (response == "Update Pet")
                {
                    await App.Current.MainPage.Navigation.PushAsync(new AddPetsPage(pet));
                }
                else if (response == "Delete Pet")
                {
                    IsBusy = true;
                    bool deleteResponse = await _petService.DeletePet(pet.Key);
                    if (deleteResponse)
                    {
                        GetFoundPets();
                    }
                }
            }
        });
        #endregion
    }
}
