using LostAndFoundPatras.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFoundPatras.Services.Interfaces
{
    public interface IPetService
    {
        Task<bool> AddOrUpdatePet(PetModel petModel);
        Task<bool> DeletePet(string key);
        Task<List<PetModel>> GetLostPets();
        Task<List<PetModel>> GetFoundPets();
    }
}
