using BrandMicroservice.src.Data;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Repository.Interfaces;
using DocumentFormat.OpenXml.Math;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BrandMicroservice.src.Repository
{
    public class MakeRepository : IMakeRepository
    {
        private readonly DatabaseContext _dbContext;
        public MakeRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> CreateMakeAsync(Make make)
        {
            await this._dbContext.Makes.AddAsync(make);
            var res = this._dbContext.SaveChanges();

            return true;
        }

        public async Task<Make?> FindMakeByMakeCodeAsync(string makeCode)
        {
            try
            {
                var make = await (from ma in this._dbContext.Makes
                           where ma.MakeCode == makeCode
                                  select ma).FirstOrDefaultAsync();

                return make;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Pagination<Make>> FindMakesAsync(MakeQuery makeQuery)
        {
            var page = makeQuery.Page ?? 1;
            var limit = makeQuery.Limit ?? 10;
            var skip = (page - 1) * limit;

            var makeName = makeQuery.MakeName;
            var makeCode = makeQuery.MakeCode;


            IQueryable<Make> query = this._dbContext.Makes;

            if (!string.IsNullOrWhiteSpace(makeName))
            {
                query = query.Where(m => EF.Functions.ILike(m.MakeName, $"%{makeName}%"));
            }

            if (!string.IsNullOrWhiteSpace(makeCode))
            {
                query = query.Where(m => EF.Functions.ILike(m.MakeCode, $"%{makeCode}%"));
            }

            var totalCount = query.Count();

            var res = await (from make in query
                             orderby make.MakeName
                             select new Make()
                             {
                                 Id = make.Id,
                                 MakeName = make.MakeName,
                                 MakeAbbreviation = make.MakeAbbreviation,
                                 MakeCode = make.MakeCode,
                                 MakeLogo = make.MakeLogo,
                                 Active = make.Active,
                                 Company = make.Company,
                                 YearFounded = make.YearFounded,
                                 Origin = make.Origin,
                                 Models = make.Models,
                             })
                             .Skip(skip)
                             .Take(limit)
                             .ToListAsync();
                             

            return new Pagination<Make>
            {
                Data = res,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
            };
        }

        public async Task<Make?> FindMakeByMakeNameAsync(string makeName)
        {
            var makeData = await (from make in this._dbContext.Makes
                                  where make.MakeName == makeName
                                  select make
                                 ).FirstOrDefaultAsync();
            return makeData;
        }

        public async Task<List<MakeNameList>>GetMakeNameListAsync()
        {
           var makeNameListData = await this._dbContext.Makes
                                    //.Where(m=>m.Active == true)
                                    .Select(m => new MakeNameList() { MakeCode = m.MakeCode, MakeName = m.MakeName })
                                    .OrderBy(m => m.MakeName)
                                    .ToListAsync();

            //var makeNameListData = await (from make in this._dbContext.Makes
            //                             //where make.Active == true
            //                             orderby make.MakeName ascending
            //                             select  new MakeNameList { MakeCode = make.MakeCode, MakeName = make.MakeName }).ToListAsync();
            return makeNameListData;
        }

        public async Task<bool> ActivateMakeAsync(string makeCode)
        {
            var makeData = await (from make in this._dbContext.Makes
                                  where make.MakeCode == makeCode
                                  select make).FirstOrDefaultAsync();
            if (makeData != null) 
            {
                makeData.Active = true;
                makeData.DateUpdated = DateTime.UtcNow;
                this._dbContext.Update(makeData);
                this._dbContext.SaveChanges();

                return true;
            }

            return false;     
        }

        public async Task<bool> UpdateMakeAsync(Guid id, UpdateMakeDto updateData)
        {
            var makeData = await (from make in this._dbContext.Makes
                                  where make.Id == id
                                  select make).FirstOrDefaultAsync();

            if (makeData != null)
            {
                makeData.MakeName = updateData.MakeName;
                makeData.MakeLogo = updateData.MakeLogo;
                makeData.YearFounded = updateData.YearFounded;
                makeData.MakeAbbreviation = updateData.MakeAbbreviation;
                makeData.Company = updateData.Company;
                makeData.Origin = updateData.Origin; 
                makeData.DateUpdated = DateTime.UtcNow;

                this._dbContext.Update(makeData);
                await this._dbContext.SaveChangesAsync();
            }
                                       
            return true;
        }

        public async Task<Make?> FindByMakeAbbreviationAsync(string makeAbbv)
        {
            var data = await (from make in this._dbContext.Makes
                              where make.MakeAbbreviation == makeAbbv
                              select make).FirstOrDefaultAsync();
            return data;
        }

        public async Task<bool> CreateMultipleMakesAsync(List<Make> makes)
        {
            await this._dbContext.Makes.AddRangeAsync(makes);
            await this._dbContext.SaveChangesAsync();
            return true;
        }

        public async Task ActivateMultipleMakes(string[] makeCodes)
        {
    
            var makesData = await (from make in this._dbContext.Makes
                            where makeCodes.Contains(make.MakeCode)
                            select make).ToListAsync();

            if(makesData.Count() > 0)
            {
                foreach(var make in makesData)
                {
                    make.Active = true;
                }

                this._dbContext.Makes.UpdateRange(makesData);
                await this._dbContext.SaveChangesAsync();
            }
            

        }
    }
}
