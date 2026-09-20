using BrandMicroservice.src.Data;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;
using BrandMicroservice.src.Models.Dtos.Model;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BrandMicroservice.src.Repository
{
    public class ModelRepository : IModelRepository
    {
        public readonly DatabaseContext _dbContext;
        public ModelRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> ActivateModelAsync(string modelCode)
        {
            try {
                var modelData = await (from model in this._dbContext.Models
                                       where model.ModelCode == modelCode
                                       select model).FirstOrDefaultAsync();

                if (modelData != null)
                {
                    modelData.Active = true;
                    this._dbContext.Update(modelData);
                    this._dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception error) {
                throw new Exception(error.Message);
            }
        }

        public async Task<bool> CreateModelAsync(Model model)
        {
            await this._dbContext.Models.AddAsync(model);
            var res = this._dbContext.SaveChanges();
            return true;
        }

        public async Task CreateMultipleModels(List<Model> models)
        {
            await this._dbContext.Models.AddRangeAsync(models);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<bool?> DeleteModelAsync(Guid id)
        {
            var res = await this._dbContext.Models.Where(mo => mo.Id == id).ExecuteDeleteAsync();

            return res > 0 ? true : false;
        }

        async public Task<ModelDto?> FindModelByModelCodeAsync(string modelCode)
        {
            try
            {
                var res = await this._dbContext.Models
                                    .Select(m => new ModelDto
                                    {
                                        Id = m.Id,
                                        ModelName = m.ModelName,
                                        ModelCode = m.ModelCode,
                                        MakeId = m.MakeId,
                                        Active = m.Active,
                                        BodyType = m.BodyType,
                                        YearFounded = m.YearFounded,
                                        Make = new MakeDto
                                        {
                                            Id = m.Makes != null ? m.Makes.Id : Guid.Empty,
                                            MakeName = m.Makes != null ? m.Makes.MakeName : "",
                                            YearFounded = m.Makes != null ? m.Makes.YearFounded : 0,
                                            Active = m.Makes != null ? m.Makes.Active : false,
                                            Company = m.Makes != null ? m.Makes.Company : "",
                                            Origin = m.Makes != null ? m.Makes.Origin : "",
                                            MakeLogo = m.Makes != null ? m.Makes.MakeLogo : "",
                                            MakeAbbreviation = m.Makes != null ? m.Makes.MakeAbbreviation : "",
                                            MakeCode = m.Makes != null ? m.Makes.MakeCode : "",
                                        }
                                    })
                                    .Where(m=>m.ModelCode == modelCode)
                                    .FirstOrDefaultAsync();
                return res;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Model?> FindModelByModelName(string modelName)
        {
            try
            {
                var modelData = await (from model in this._dbContext.Models
                                      where model.ModelName == modelName
                                      select model).FirstOrDefaultAsync();
                return modelData;                                      
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Pagination<Model>> FindModelsAsync(ModelQuery modelQuery)
        {
            var page = modelQuery.Page ?? 1;
            var limit = modelQuery.Limit ?? 10;
            var skip = (page - 1) * limit;

            var modelName = modelQuery.ModelName;
            var makeName = modelQuery.MakeName;
            var modelCode = modelQuery.ModelCode;
            var active = modelQuery.Active;
            var yearFounded = modelQuery.YearFounded;
            var bodyType = modelQuery.BodyType;
            var makeCode = modelQuery.MakeCode;

            IQueryable<Model> query = this._dbContext.Models;

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                query = query.Where(m => EF.Functions.ILike(m.ModelName, $"%{modelName}%"));
            }

            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                query = query.Where(m => EF.Functions.ILike(m.ModelCode, $"%{modelCode}%"));
            }

            if (!string.IsNullOrWhiteSpace(makeName))
            {
                query = query.Where(m => m.Makes != null && EF.Functions.ILike(m.Makes.MakeName, $"%{makeName}%"));
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                var activeData = active == "true" ? true : false;
                query = query.Where(m => m.Active == activeData);
            }

            if (yearFounded.HasValue)
            {
                query = query.Where(m => m.YearFounded == yearFounded);
            }

            if (!string.IsNullOrWhiteSpace(bodyType))
            {
                query = query.Where(m => m.BodyType == bodyType);
            }

            if (!string.IsNullOrWhiteSpace(makeCode))
            {
                query = query.Where(m => m.Makes != null && EF.Functions.ILike(m.Makes.MakeCode, $"%{makeCode}%"));
            }

            var totalCount = query.Count();

            var res = await query
                    .Include(m => m.Makes)
                    .Select(m => new Model()
                    {
                        Id = m.Id,
                        ModelName = m.ModelName,
                        Active = m.Active,
                        BodyType = m.BodyType,
                        YearFounded = m.YearFounded,
                        MakeId = m.MakeId,
                        ModelCode = m.ModelCode,
                        Makes = m.Makes
                    })
                    .OrderBy(m => m.ModelName)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync();

            //var res = await (from model in this._dbContext.Models
            //                 join make in this._dbContext.Makes on model.MakeId equals make.Id
            //                 select new Model()
            //                 {
            //                     Id = model.Id,
            //                     ModelName = model.ModelName,
            //                     Active = model.Active,
            //                     BodyType = model.BodyType,
            //                     YearFounded = model.YearFounded,
            //                     MakeId = model.MakeId,
            //                     ModelCode = model.ModelCode,
            //                     Makes = model.Makes
            //                 }).ToListAsync();

            return new Pagination<Model> 
            { 
                Data = res,
                TotalCount = totalCount,
                Page = page,
                Limit = limit
            };
        }

        public async Task<List<ModelFilterDto>> GetModelFilterAsync(List<string> makes)
        {
            try
            {
                IQueryable<Model> query = this._dbContext.Models;
                query = query.Where(c => c.Makes != null ? makes.Contains(c.Makes.MakeName) : false);

                var res = await query
                            .Select(m=>new ModelFilterDto
                            { 
                                ModelName = m.ModelName,
                                MakeName = m.Makes!.MakeName,
                                ModelCode = m.ModelCode,
                                Total = 0
                            })
                            .OrderBy(c=>c.ModelName)
                            .ToListAsync();

                return res;
                                       
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ModelByMakeNameList>> GetModelsByMakeName(string makeName)
        {

            var res = await this._dbContext.Models
                .Where(mo=>mo.Makes != null && mo.Makes.MakeName == makeName)
                .Join(this._dbContext.Makes,
                mo => mo.MakeId,
                ma => ma.Id,
                (mod, mak) => new ModelByMakeNameList { ModelCode = mod.ModelCode, ModelName = mod.ModelName, BodyType = mod.BodyType })
                .OrderBy(m => m.ModelName)
                .ToListAsync();

            return res;
        }

        public async Task<bool> UpdateModelAsync(Guid id, UpdateModelDto updateModel)
        {
            var data = await (from model in this._dbContext.Models
                              where model.Id == id
                              select model).FirstOrDefaultAsync();

            if (data != null)
            {
                data.ModelName = updateModel.ModelName;
                data.Active = updateModel.Active;
                data.BodyType = updateModel.BodyType;
                data.YearFounded = updateModel.YearFounded;
                this._dbContext.Update(data);
                this._dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
