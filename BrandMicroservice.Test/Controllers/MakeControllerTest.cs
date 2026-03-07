using BrandMicroservice.src.Controllers;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Repository.Interfaces;
using BrandMicroservice.src.Services.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BrandMicroservice.Test.Controllers
{
    public class MakeControllerTest
    {
        private readonly Mock<IMakeService> _MakeService;

        private readonly MakeController _MakeController;

        public MakeControllerTest()
        {
            _MakeService = new Mock<IMakeService>();

            _MakeController = new MakeController(_MakeService.Object);
        }

        [Fact]
        public async Task GetMakes_ReturnMakes()
        {
            List<Make> makesMockDataList = new List<Make>()
            {
                new Make()
                {
                    Id = Guid.NewGuid(),
                    MakeName = "Chevrolet",
                    Active = true,
                    MakeAbbreviation = "CHE",
                    MakeLogo = "",
                    Company = "General Motors",
                    Models = [],
                    DateCreated = DateTime.Now,
                    MakeCode = "CHE-12345678",
                    Origin = "USA",
                    YearFounded = 1920
                },
                new Make()
                {
                    Id = Guid.NewGuid(),
                    MakeName = "Chevrolet",
                    Active = true,
                    MakeAbbreviation = "CHE",
                    MakeLogo = "",
                    Company = "General Motors",
                    Models = [],
                    DateCreated = DateTime.Now,
                    MakeCode = "CHE-12345678",
                    Origin = "USA",
                    YearFounded = 1920
                }
            };

            //_MakeService
            //    .Setup(m => m.GetMakes())
            //    .ReturnsAsync(makesMockDataList);

            var result = await _MakeController.GetMakes();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(okResult.StatusCode, 200);
            okResult.Value.Should().BeEquivalentTo(makesMockDataList);
        }

        [Fact]
        public async Task CreateMake_MakeRequestBody_ReturnTrue()
        {
            MakeRequestBody bodyData = new MakeRequestBody
            {
                MakeName = "Ford",
                MakeAbbreviation = "FOR",
                MakeLogo = "",
                Origin = "USA",
                YearFounded = 1999,
                Company = "General Motors"
            };

            _MakeService
                .Setup(m=>m.CreateMake(bodyData))
                .ReturnsAsync(true);

            var res = await _MakeController.CreateMake(bodyData);

            var okResult = Assert.IsType<OkObjectResult>(res.Result);
            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, true);
            // okResult.Value.Should().Be(true);
        }

        [Fact]
        public void DownloadMakesTemplateTest()
        {
          _MakeService
                .Setup(m => m.DownloadMakesTemplate())
                .Returns(new byte[] { 1, 2, 3, 4 });

         var result = _MakeController.DownloadMakesTemplate();

         var httpActionResult = Assert.IsType<FileContentResult>(result);
         Assert.Equal(httpActionResult.ContentType, string.Format($"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            httpActionResult.FileContents.Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4 });
        }

        [Fact]
        public async Task  ActivateMakeTest()
        {
            string mockMakeCode = "CHE-12345678";
            var mockResData = new ActivateMakeDto { MakeCode = "CHE-12345678", Status = "Active", MakeName = "Chevrolet" };

            this._MakeService
                .Setup(m=>m.ActivateMake(mockMakeCode))
                .ReturnsAsync(mockResData);

            var result = await this._MakeController.ActivateMake(mockMakeCode);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);

            okResult.Value.Should().BeEquivalentTo(mockResData);
        }

        [Fact]
        public async Task GetMakesNameList()
        {
            List<MakeNameList> MockMakeListData = new List<MakeNameList>
            {
                new MakeNameList
                {
                    MakeName = "Honda",
                    MakeCode = "HON-12345678"
                },
                new MakeNameList
                {
                    MakeName = "Ford",
                    MakeCode = "FOR-12345678"
                }
            };
            this._MakeService
                .Setup(m => m.GetMakeNameListAsync())
                .ReturnsAsync(MockMakeListData);

            var result = await this._MakeController.GetMakeNameList();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            okResult.StatusCode.Should().Be(200);

            okResult.Value.Should().BeEquivalentTo(MockMakeListData);
        }
    }
}
