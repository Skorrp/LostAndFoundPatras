using LostAndFoundPatras.Models;
using LostAndFoundPatras.Services.Implementations;
using LostAndFoundPatras.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LostAndFoundPatras.ViewModels
{
    public class AddPetsPageViewModel : BaseViewModel
    {
        #region Properties
        private readonly IPetService _petService;

        private PetModel _petDetail = new PetModel();
        public PetModel PetDetail
        {
            get => _petDetail;
            set => SetProperty(ref _petDetail, value);
        }
        #endregion

        #region Constructor
        public AddPetsPageViewModel()
        {
            _petService = DependencyService.Resolve<IPetService>();
        }

        public AddPetsPageViewModel(PetModel petResponse)
        {
            _petService = DependencyService.Resolve<IPetService>();
            PetDetail = new PetModel
            {
                Found = petResponse.Found,
                Lost = petResponse.Lost,
                Date = petResponse.Date,
                Description = petResponse.Description,
                Area = petResponse.Area,
                Photo = petResponse.Photo,
                Email = petResponse.Email,
                PhoneNumber = petResponse.PhoneNumber,
                Key = petResponse.Key
            };
            //OnPropertyChanged(nameof(petResponse.Date));
        }
        #endregion

        #region Commands
        public ICommand SavePetCommand => new Command(async () =>
        {
            if (IsBusy) return;
            IsBusy = true;
            bool res = await _petService.AddOrUpdatePet(PetDetail);
            if (res)
            {
                if (!string.IsNullOrWhiteSpace(PetDetail.Key))
                {
                    await App.Current.MainPage.DisplayAlert("Success!", "Record Updated successfully.", "Ok");
                }
                else
                {
                    PetDetail = new PetModel() { };
                    await App.Current.MainPage.DisplayAlert("Success!", "Record added successfully.", "Ok");
                }
            }
            IsBusy = false;
        });
        #endregion
    }
}
