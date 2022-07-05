using Firebase.Database;
using Firebase.Database.Query;
using LostAndFoundPatras.Content;
using LostAndFoundPatras.Models;
using LostAndFoundPatras.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFoundPatras.Services.Implementations
{
    public class PetService : IPetService
    {
        FirebaseClient firebase = new FirebaseClient(Settings.FireBaseDatabaseUrl, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(Settings.FireBaseSecret)
        });

        public async Task<bool> AddOrUpdatePet(PetModel petModel)
        {
            if (!string.IsNullOrWhiteSpace(petModel.Key))
            {
                try
                {
                    await firebase.Child(nameof(PetModel)).Child(petModel.Lost).Child(petModel.Key).PutAsync(petModel);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                var response = await firebase.Child(nameof(PetModel)).Child(petModel.Lost).PostAsync(petModel);
                if (response.Key != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> DeletePet(string Key)
        {
            try
            {
                await firebase.Child(nameof(PetModel)).Child(Key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<PetModel>> GetLostPets()
        {
            {
                return (await firebase.Child(nameof(PetModel)).Child(nameof(PetModel.Lost)).OnceAsync<PetModel>()).Select(f => new PetModel
                {
                    Lost = f.Object.Lost,
                    Date = f.Object.Date,
                    Description = f.Object.Description,
                    Area = f.Object.Area,
                    Photo = f.Object.Photo,
                    Email = f.Object.Email,
                    PhoneNumber = f.Object.PhoneNumber,
                    Key = f.Key
                }).ToList();
            }
        }
        public async Task<List<PetModel>> GetFoundPets()
        {
            {
                return (await firebase.Child(nameof(PetModel)).Child(nameof(PetModel.Found)).OnceAsync<PetModel>()).Select(f => new PetModel
                {
                    Found = f.Object.Found,
                    Date = f.Object.Date,
                    Description = f.Object.Description,
                    Area = f.Object.Area,
                    Photo = f.Object.Photo,
                    Email = f.Object.Email,
                    PhoneNumber = f.Object.PhoneNumber,
                    Key = f.Key
                }).ToList();
            }
        }
    }
}
