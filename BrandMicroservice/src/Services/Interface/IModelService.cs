using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Model;
using BrandMicroservice.src.Models.Dtos.Shared;
using System.Threading.Tasks;

namespace BrandMicroservice.src.Services.Interface
{
    public interface IModelService
    {
        Task<Pagination<ModelDto>> GetModels(ModelQuery modelQuery);

        Task<bool> CreateModel(ModelRequestBody model);

        Task<Model> GetModelByModelCode(string modelCode);

        Task<List<ModelByMakeNameList>> GetModelsByMakeName(string makeName);

        Task<bool> UpdateModel(string modelCode, UpdateModelBodyDto body);

        Task<ActivateModelDto> ActivateModel(string modelCode);

        Task<string> CreateMultipleModels(CreateMultipleModelsDto file);

        byte[] DownloadModelsTemplate();

    }
}
