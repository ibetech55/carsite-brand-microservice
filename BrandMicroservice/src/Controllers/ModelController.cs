using BrandMicroservice.src.Configs;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Model;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BrandMicroservice.src.Controllers
{
    [ApiController]
    [Route("api/brand-microservice/model")]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            this._modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ModelDto>>> GetModels([FromQuery] ModelQuery modelQuery)
        {
            return Ok(await this._modelService.GetModels(modelQuery));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateModel(ModelRequestBody body)
        {
            return Ok(await this._modelService.CreateModel(body));
        }

        [HttpGet("{modelCode}")]
        public async Task<ActionResult<Model>> GetModelByModelCode(string modelCode)
        {
            return Ok(await this._modelService.GetModelByModelCode(modelCode));
        }

        [HttpGet("modelsByMakeName/{makeName}")]
        public async Task<ActionResult<IEnumerable<ModelByMakeNameList>>> GetModelsByMakeName(string makeName)
        {
            var res = await this._modelService.GetModelsByMakeName(makeName);
            return Ok(res);
        }

        [HttpPut("{modelCode}")]
        public async Task<ActionResult<bool>> UpdateMake(string modelCode, UpdateModelBodyDto body)
        {
            var res = await this._modelService.UpdateModel(modelCode, body);
            if(res == true)
            {
                return Ok(res);
            } else
            {
                return BadRequest();
            }
        }

        [HttpPatch("activateModel/{modelCode}")]
        public async Task<ActionResult<ActivateModelDto>> ActivateModel(string modelCode)
        {
            var res = await this._modelService.ActivateModel(modelCode);
            return Ok(res);
        }

        [HttpPost("createMultipleModels")]

        public async Task<ActionResult> CreateMultipleModels([FromForm] CreateMultipleModelsDto body)
        {
            if (body.FileData.ContentType != Constants.SPREADSHEET_FILE_TYPE)
            {
                throw new ArgumentException("Incorrect file type, file type must be a spreadsheet");
            }

            var res = await this._modelService.CreateMultipleModels(body);
            return Ok(new
            {
                data = res
            });
        }

        [HttpGet("downloadModelsTemplate")]
        public ActionResult DownloadMakesTemplate()
        {
            var fileBytes = this._modelService.DownloadModelsTemplate();

            return File(
                fileBytes,
                Constants.SPREADSHEET_FILE_TYPE,
                "models_template.xlsx"
            );
        }
    }
}
