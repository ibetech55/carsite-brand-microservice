using BrandMicroservice.src.Configs;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Models.Dtos.MakeDTOs;
using BrandMicroservice.src.Models.Dtos.Shared;
using BrandMicroservice.src.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BrandMicroservice.src.Controllers
{
    [ApiController]
    [Route("api/brand-microservice/make")]
    public class MakeController : ControllerBase
    {
        private readonly IMakeService _makeService;
        public MakeController(IMakeService makeService)
        {
            this._makeService = makeService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<MakeDto>>> GetMakes([FromQuery] MakeQuery makeQuery)
        {
            var res = await this._makeService.GetMakes(makeQuery);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateMake(MakeRequestBody make)
        {
            var res = await this._makeService.CreateMake(make);
            return res == true ? Ok(res) : BadRequest();
        }

        [HttpGet("{makeCode}")]
        public async Task<ActionResult<Make>> GetMakeByMakeCode(string makeCode)
        {
            Make make = await this._makeService.GetMakeByMakeCode(makeCode);
            return Ok(make);
        }

        [HttpGet("makeName/{makeName}")]
        public async Task<ActionResult<Make>> GetMakeByMakeName(string makeName)
        {
            Make make = await this._makeService.GetMakeByMakeName(makeName);
            return Ok(make);
        }

        [HttpGet("makeNameList")]
        public async Task<ActionResult<List<MakeNameList>>> GetMakeNameList()
        {
            var data = await this._makeService.GetMakeNameListAsync();

            return Ok(data);
        }

        [HttpPatch("activateMake/{makeCode}")]
        public async Task<ActionResult<ActivateMakeDto>> ActivateMake(string makeCode)
        {
            var data = await this._makeService.ActivateMake(makeCode);

            return Ok(data);
        }

        [HttpPut("{makeCode}")]
        public async Task<ActionResult<bool>> UpdateMake(string makeCode, UpdateMake body)
        {
            var res = await this._makeService.UpdateMake(makeCode, body);
            return Ok(res);
        }

        [HttpPost("createMultipleMakes")]
        public async Task<ActionResult> CreateMultipleMakes([FromForm] CreateMultipleMakeDto body)
        {
            if(body.FileData.ContentType != Constants.SPREADSHEET_FILE_TYPE)
            {
                throw new ArgumentException("Incorrect file type, file type must be a spreadsheet");
            }

            var res = await this._makeService.CreateMultipleMakes(body);
            return Ok(new { data = res});
        }

        [HttpGet("downloadMakesTemplate")]
        public FileContentResult DownloadMakesTemplate()
        {
            var fileBytes = this._makeService.DownloadMakesTemplate();

            return File(
                fileBytes,
                Constants.SPREADSHEET_FILE_TYPE,
                "makes_template.xlsx"
            );
        }

        [HttpPatch("activateMultiple")]

        public async Task<ActionResult> ActivateMultipleMakes([FromBody] ActivateMultipleMakesDto makeCodes)
        {
            if(makeCodes.Makes != null)
            {
                await this._makeService.ActivateMultipleMakes(makeCodes.Makes);
            }
           

            return Ok(new
            {
                data = "Transaction Successful"
            });
        }
    }
}
