using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;
using BrandMicroservice.src.Models.Dtos.Shared;

namespace BrandMicroservice.src.Repository.Interfaces
{
    public interface IMakeRepository
    {
        Task<Pagination<Make>> FindMakesAsync(MakeQuery makeQuery);
        Task<bool> CreateMakeAsync(Make make);

        Task<Make?> FindMakeByMakeCodeAsync(string makeCode);

        Task<Make?> FindMakeByMakeNameAsync(string name);

        Task<List<MakeNameList>> GetMakeNameListAsync();

        Task<bool> ActivateMakeAsync(string makeCode);

        Task<bool> UpdateMakeAsync(Guid id, UpdateMakeDto makeData);

        Task<Make?> FindByMakeAbbreviationAsync(string makeAbbv);

        Task<bool> CreateMultipleMakesAsync(List<Make> makes);

        Task ActivateMultipleMakes(string[] makeCode);
    }
}
