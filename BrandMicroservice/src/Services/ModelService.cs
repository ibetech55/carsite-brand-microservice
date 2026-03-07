using AutoMapper;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Models.Dtos.Model;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Repository;
using BrandMicroservice.src.Repository.Interfaces;
using BrandMicroservice.src.Services.Interface;
using BrandMicroservice.src.Utils;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BrandMicroservice.src.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMakeRepository _makeRepository;
        private readonly IMapper _Mapper;
        public ModelService(IModelRepository modelRepository, IMakeRepository makeRepository, IMapper mapper)
        {
            this._modelRepository = modelRepository;
            this._makeRepository = makeRepository;
            this._Mapper = mapper;
        }
        public async Task<bool> CreateModel(ModelRequestBody body)
        {
            Make? make = await this._makeRepository.FindMakeByMakeCodeAsync(body.MakeCode);

            if(make == null)
            {
                throw new ArgumentException("MakeCode does not exist");
            }

            var modelCode = String.Format($"{make.MakeAbbreviation}-{GenerateCode.Execute(6)}");

            var bodyTypeFormatted = String.Join(",", body.BodyType);

            Model newModel = new Model()
            {
                ModelName = body.ModelName,
                MakeId = make.Id,
                Active = false,
                BodyType = bodyTypeFormatted,
                YearFounded = body.YearFounded,
                ModelCode = modelCode
            };

            return await this._modelRepository.CreateModelAsync(newModel);
        }

        public async Task<Pagination<ModelDto>> GetModels(ModelQuery modelQuery)
        {
            var data = await this._modelRepository.FindModelsAsync(modelQuery);
            return new Pagination<ModelDto>
            {
                Data = this._Mapper.Map<List<ModelDto>>(data.Data),
                Limit = data.Limit,
                Page = data.Page,
                TotalCount = data.TotalCount
            };
        }

        public async Task<Model> GetModelByModelCode(string modelCode)
        {
            var res = await this._modelRepository.FindModelByModelCodeAsync(modelCode);

            if (res == null) 
            {
                throw new ArgumentException("Model not found");
            }

            return res;
        }

        public async Task<List<ModelByMakeNameList>> GetModelsByMakeName(string makeName)
        {
            var makeData = await this._makeRepository.FindMakeByMakeNameAsync(makeName);

            if(makeData == null)
            {
                throw new ArgumentException($"make name {makeName} does not exist");
            }
            var data = await this._modelRepository.GetModelsByMakeName(makeName);

            return data;
        }

        public async Task<bool> UpdateModel(string modelCode, UpdateModelBodyDto body)
        {
            var modelData = await this._modelRepository.FindModelByModelCodeAsync(modelCode);

            if (modelData == null)
            {
                throw new ArgumentException($"{modelCode} does not exist");
            }

            var data = new UpdateModelDto()
            {
                ModelName = body.ModelName ?? modelData.ModelName,
                Active = body.Active != null ? body.Active.Value : modelData.Active,
                BodyType = body.BodyType ?? modelData.BodyType,
                YearFounded = body.YearFounded != null ? body.YearFounded.Value : modelData.YearFounded
            };

            var res = await this._modelRepository.UpdateModelAsync(modelData.Id, data);

            return res;
        }

        public async Task<ActivateModelDto> ActivateModel(string modelCode)
        {
            var modelData = await this._modelRepository.FindModelByModelCodeAsync(modelCode);

            if(modelData == null)
            {
                throw new ArgumentException($"{modelCode} does not exist");
            }

            var res = await this._modelRepository.ActivateModelAsync(modelCode);

            return new ActivateModelDto()
            {
                ModelCode = modelData.ModelCode,
                Status = "Active",
                ModelName = modelData.ModelName,
            };
        }

        public async Task<string> CreateMultipleModels(CreateMultipleModelsDto file)
        {
            List<string> errorList = new List<string>() { };
            List<string> checkHeaders = new List<string>() { };
            List<Model> newModels = new List<Model>() { };
            List<string> headersList = new List<string>() 
            {
                "Model Name",
                "Year Founded",
                "Make Name",
                "Body Type",
            };

            using(var stream = new MemoryStream())
            {
                await file.FileData.CopyToAsync(stream);
                stream.Position = 0;
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var headersRow = worksheet.FirstRowUsed();

                    if (headersRow != null)
                    {
                        var headers = headersRow.Cells().Select(c => c.GetString().Trim()).ToList();


                        foreach (var header in headers)
                        {
                            if (!headersList.Contains(header))
                            {
                                errorList.Add(header);
                            }
                            else
                            {
                                checkHeaders.Add(header);
                            }
                        }

                        if (errorList.Count() > 0)
                        {
                            var listData = string.Join(", ", errorList);
                            throw new ArgumentException($"Incorrect file headers {listData}");
                        }

                        if (!headers.SequenceEqual(checkHeaders))
                        {
                            throw new ArgumentException($"Incorrect or missing file headers");
                        }
                    }


                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var modelName = row.Cell(1).GetString();
                        var yearFounded = row.Cell(2).GetValue<int>();
                        var makeName = row.Cell(3).GetString();
                        var bodyType = row.Cell(4).GetString();


                        var makeData = await this._makeRepository.FindMakeByMakeNameAsync(makeName);

                        if (makeData == null)
                        {
                            throw new ArgumentException($"{makeData?.MakeName} does not exists");
                        }

                        var modelData = await this._modelRepository.FindModelByModelName(modelName);

                        if (modelData != null)
                        {
                            throw new ArgumentException($"{modelName} already exists");
                        }

                        var modelCode = String.Format($"{makeData.MakeAbbreviation}-{GenerateCode.Execute(5)}");

                        var checkModelCode = this.GetModelByModelCode(modelCode);

                        if (checkModelCode != null)
                        {
                            throw new ArgumentException($"{modelCode} already exists");                          
                        }

                        newModels.Add(new Model()
                        {
                            ModelName = modelName,
                            BodyType = bodyType,
                            YearFounded = yearFounded,
                            MakeId = makeData.Id,
                            ModelCode = String.Format($"{makeData.MakeAbbreviation}-{GenerateCode.Execute(5)}"),
                            Active = false
                        });
                    }

                    await this._modelRepository.CreateMultipleModels(newModels);

                    return "Transaction Successful";
                }
            }
        }

        public byte[] DownloadModelsTemplate()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Models");

                worksheet.Cell(1, 1).Value = "Model Name";
                worksheet.Cell(1, 2).Value = "Year Founded";
                worksheet.Cell(1, 3).Value = "Make Name";
                worksheet.Cell(1, 4).Value = "Body Type";

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return memoryStream.ToArray();
                }

            }
        }
    }
}
