using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Model;
using BrandMicroservice.src.Models.Dtos.Shared;

namespace BrandMicroservice.src.Repository.Interfaces
{
    public interface IModelRepository
    {
        Task<Pagination<Model>> FindModelsAsync(ModelQuery modelQuery);

        Task<bool> CreateModelAsync(Model model);

        Task<Model?> FindModelByModelCodeAsync(string modelCode);

        Task<Model?> FindModelByModelName(string modelName);

        Task<List<ModelByMakeNameList>> GetModelsByMakeName(string makeName);

        Task<bool?> DeleteModelAsync(Guid id);

        Task<bool> UpdateModelAsync(Guid id, UpdateModelDto updateModel);

        Task<bool> ActivateModelAsync(string modelCode);

        Task CreateMultipleModels(List<Model> models);
    }
}
