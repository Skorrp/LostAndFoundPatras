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
using System.Diagnostics;

namespace LostAndFoundPatras.ViewModels
{
    public class PetListPageViewModel: BaseViewModel
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
        public PetListPageViewModel()
        {
            _petService = DependencyService.Resolve<IPetService>();
            GetLostPets();
        }
        #endregion

        #region Methods
        private void GetLostPets()
        {
            IsBusy = true;
            Task.Run(async () =>
            { 
                var petList = await _petService.GetLostPets();

                Device.BeginInvokeOnMainThread(() =>
                {

                    Pets.Clear();
                    if (petList?.Count > 0)
                    {
                        foreach (var pet in petList)
                        {
                            Pets.Add(pet);
                            Debug.WriteLine($"This Petition's Location is X:{pet.Latitude}, Y:{pet.Longitude}");
                            MessagingCenter.Send(pet, "LocationData");
                            //if (pet.Latitude == null)
                            //{
                            //    pet.Latitude = "0";
                            //}
                            //else
                            //{
                            //    string _pet = pet.Latitude.ToString();
                            //    MessagingCenter.Send("this", "abcd", _pet.ToString());
                            //}
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
            GetLostPets();
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
                        GetLostPets();
                    }
                }
            }
        });
        #endregion
    }
}
