using ClosedXML.Excel;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Repository.Interfaces;
using BrandMicroservice.src.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BrandMicroservice.src.Utils;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;

namespace BrandMicroservice.src.Services
{
    public class MakeService : IMakeService
    {
        private readonly IMakeRepository _makeRepository;
        private readonly IMapper _Mapper;
        public MakeService(IMakeRepository makeRepository, IMapper Mapper)
        {
            this._makeRepository = makeRepository;
            this._Mapper = Mapper;
        }

        public async Task<ActivateMakeDto> ActivateMake(string makeCode)
        {
            var makeData = await this._makeRepository.FindMakeByMakeCodeAsync(makeCode);

            if(makeData == null)
            {
                throw new ArgumentException($"Make code {makeCode} does not exist");
            }

            await this._makeRepository.ActivateMakeAsync(makeCode);

            return new ActivateMakeDto() { MakeCode = makeCode, MakeName = makeData.MakeName, Status = "Active" };
        }

        public async Task ActivateMultipleMakes(string[] makeCodes)
        {
            foreach(var makeCode in makeCodes)
            {
                var makeData = await this._makeRepository.FindMakeByMakeCodeAsync(makeCode);

                if(makeData == null) 
                {
                    throw new ArgumentException($"{makeCode} does not exist");
                }
            }
            await this._makeRepository.ActivateMultipleMakes(makeCodes);
        }

        public  async Task<bool> CreateMake(MakeRequestBody body)
        {
            var makeData = await this._makeRepository.FindMakeByMakeNameAsync(body.MakeName);

            if (makeData != null)
            {
                throw new ArgumentException($"{body.MakeName} already exsits");
            }

            var makeAbbrCheck = await this._makeRepository.FindByMakeAbbreviationAsync(body.MakeAbbreviation);

            if (makeAbbrCheck != null)
            {
                throw new ArgumentException($"makeAbbreviation {body.MakeAbbreviation} already exsits");
            }

            string makeAbbv = body.MakeAbbreviation.ToUpper();
            string makeCode = GenerateCode.Execute(8);
            var newMake = new Make() 
            { 
                MakeName = body.MakeName,
                Origin = body.Origin,
                MakeLogo = body.MakeLogo.Name,
                Active = false,
                YearFounded = body.YearFounded,
                MakeCode = this.GenerateMakeCode(makeAbbv),
                MakeAbbreviation = makeAbbv,
                Company = body.Company,
            };
            var res = await this._makeRepository.CreateMakeAsync(newMake);

            return res;
        }

        public async Task<string> CreateMultipleMakes(CreateMultipleMakeDto file)
        {
            List<string> errorList = new List<string> { };
            List<string> headersList = new List<string>
            {
                "Make Name",
                "Origin",
                "Company",
                "Year Founded",
                "Make Abbreviation"
            };
            List<string> checkHeaders = new List<string> { };
            List<Make> newMakes = new List<Make> { };

            using (var stream = new MemoryStream())
            {
                await file.FileData.CopyToAsync(stream);
                stream.Position = 0;

                using(var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // first worksheet
                    var headersRow = worksheet.FirstRowUsed();

                    if(headersRow != null)
                    {
                        var headers = headersRow.Cells().Select(c => c.GetString().Trim()).ToList();


                        foreach (var header in headers)
                        {
                            if (!headersList.Contains(header))
                            {
                                errorList.Add(header);
                            } else {
                                checkHeaders.Add(header);
                            }
                        }

                        if(errorList.Count() > 0)
                        {
                            var listData = string.Join(", ", errorList);
                            throw new ArgumentException($"Incorrect file headers {listData}");
                        }

                        if(!headers.SequenceEqual(checkHeaders))
                        {
                            throw new ArgumentException($"Incorrect or missing file headers");
                        }
                    }


                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var makeName = row.Cell(1).GetString();
                        var origin = row.Cell(2).GetString();
                        var company = row.Cell(3).GetString();
                        var yearFounded = row.Cell(4).GetValue<int>();
                        var makeAbbv = row.Cell(5).GetString().ToUpper();

                        var makeData = await this._makeRepository.FindMakeByMakeNameAsync(makeName);

                        if (makeData != null)
                        {
                            throw new ArgumentException($"{makeData.MakeName} already exists");
                        }

                        var makeAbbvCheck = await this._makeRepository.FindByMakeAbbreviationAsync(makeAbbv);

                        if (makeAbbvCheck != null)
                        {
                            throw new ArgumentException($"{makeAbbvCheck.MakeAbbreviation} already exists");
                        }

                        newMakes.Add(new Make()
                        {
                            MakeName = makeName,
                            Origin = origin,
                            Company = company,
                            YearFounded = yearFounded,
                            MakeAbbreviation = makeAbbv,
                            MakeCode = this.GenerateMakeCode(makeAbbv),
                            MakeLogo = "",
                            Active = false
                        });
                    }

                    await this._makeRepository.CreateMultipleMakesAsync(newMakes);

                    return "Transaction Successful";
                }
            }
        }

        public byte[] DownloadMakesTemplate()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Makes");

                worksheet.Cell(1, 1).Value = "Make Name";
                worksheet.Cell(1, 2).Value = "Origin";
                worksheet.Cell(1, 3).Value = "Compnay";
                worksheet.Cell(1, 4).Value = "Year Founded";
                worksheet.Cell(1, 5).Value = "Make Abbreviation";

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return memoryStream.ToArray();
                }

            }
        }

        public async Task<Make> GetMakeByMakeCode(string makeCode)
        {
            Make? make = await this._makeRepository.FindMakeByMakeCodeAsync(makeCode);
            if (make == null)
            {
                throw new KeyNotFoundException("Make not found");
            }
            
            return make;
        }

        public async Task<Make> GetMakeByMakeName(string makeName)
        {
            var res = await this._makeRepository.FindMakeByMakeNameAsync(makeName);

            if(res == null)
            {
                throw new ArgumentException("Model not found");
            }

            return res;
        }

        public async Task<List<MakeNameList>> GetMakeNameListAsync()
        {
            var res = await this._makeRepository.GetMakeNameListAsync();

            return res;
        }

        public async Task<Pagination<MakeDto>> GetMakes(MakeQuery makeQuery)
        {
   
            var data = await this._makeRepository.FindMakesAsync(makeQuery);

            return new Pagination<MakeDto>
            {
                Data = this._Mapper.Map<List<MakeDto>>(data.Data),
                Limit = data.Limit,
                Page = data.Page,
                TotalCount = data.TotalCount
            };
        }

        public async Task<bool> UpdateMake(string makeCode, UpdateMake body)
        {
            var makeData = await this._makeRepository.FindMakeByMakeCodeAsync(makeCode);

            if (makeData == null)
            {
                throw new ArgumentException("Make not found");
            }

            UpdateMakeDto updateMakeData = new UpdateMakeDto()
            {
                MakeName = body.MakeName ?? makeData.MakeName,
                MakeLogo = body.MakeLogo ?? makeData.MakeLogo,
                Company = body.Company ?? makeData.Company,
                Origin = body.Origin ?? makeData.Origin,
                MakeAbbreviation = body.MakeAbbreviation ?? makeData.MakeAbbreviation,
                YearFounded = body.YearFounded.HasValue ? body.YearFounded.Value : makeData.YearFounded,
                Active = body.Active.HasValue ? body.Active.Value : makeData.Active
            };

            var res = await this._makeRepository.UpdateMakeAsync(makeData.Id, updateMakeData);
            return res;
        }

        private string GenerateMakeCode(string makeAbbv)
        {
            return String.Format($"{makeAbbv}-{GenerateCode.Execute(8)}");
        }
    }
}
