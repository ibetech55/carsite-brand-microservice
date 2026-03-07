using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;
using BrandMicroservice.src.Models.Dtos.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BrandMicroservice.src.Services.Interface
{
    public interface IMakeService
    {
        Task<Pagination<MakeDto>> GetMakes(MakeQuery makeQuery);

        Task<bool> CreateMake(MakeRequestBody body);

        Task<Make> GetMakeByMakeCode(string makeCode);

        Task<Make> GetMakeByMakeName(string makeName);

        Task<List<MakeNameList>> GetMakeNameListAsync();

        Task<ActivateMakeDto> ActivateMake(string makeCode);

        Task<bool> UpdateMake(string makeCode, UpdateMake body);

        Task<string> CreateMultipleMakes(CreateMultipleMakeDto file);

        byte[] DownloadMakesTemplate();

        Task ActivateMultipleMakes(string[] makeCode);
    }
}
