using Firebase.Database;
using Firebase.Database.Query;
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
                    await firebase.Child(nameof(PetModel)).Child(petModel.Key).PutAsync(petModel);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                var response = await firebase.Child(nameof(PetModel)).PostAsync(petModel);
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
        public async Task<List<PetModel>> GetAllPets()
        {
            return (await firebase.Child(nameof(PetModel)).OnceAsync<PetModel>()).Select(f => new PetModel
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
}
