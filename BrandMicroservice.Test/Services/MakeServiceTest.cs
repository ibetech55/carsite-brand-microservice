using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Repository.Interfaces;
using BrandMicroservice.src.Services;
using BrandMicroservice.src.Services.Interface;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace BrandMicroservice.Test.Services
{
    public class MakeServiceTest
    {
        private readonly Mock<IMakeRepository> _MakeRepository;

        private readonly IMakeService _MakeService;

        public MakeServiceTest()
        {
            this._MakeRepository = new Mock<IMakeRepository>();

            this._MakeService = new MakeService(this._MakeRepository.Object);
        }

        public Make CreateMake() => new Make()
        {
            Id = Guid.NewGuid(),
            MakeName = "Chevrolet",
            Active = false,
            MakeAbbreviation = "CHE",
            MakeLogo = "",
            Company = "General Motors",
            Models = [],
            DateCreated = DateTime.Now,
            MakeCode = "CHE-12345678",
            Origin = "USA",
            YearFounded = 1920
        };

        [Fact]
        public async Task ActivateMake_WithMakeCode_Return_ActivateData()
        {            
            // Arrange
            var makeCode = "CHE-12345678";
            ActivateMakeDto data = new ActivateMakeDto { MakeCode = "CHE-12345678", Status = "Active", MakeName = "Chevrolet" };
            var makeData = new Make()
            {
                Id = Guid.NewGuid(),
                MakeName = "Chevrolet",
                Active = false,
                MakeAbbreviation = "CHE",
                MakeLogo = "",
                Company = "General Motors",
                Models = [],
                DateCreated = DateTime.Now,
                MakeCode = "CHE-12345678",
                Origin = "USA",
                YearFounded = 1920
            };

            //Act
            this._MakeRepository
                .Setup(m => m.FindMakeByMakeCodeAsync("CHE-12345678"))
                .ReturnsAsync(makeData);

            var res = await this._MakeService.ActivateMake(makeCode);

            res.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task ActiveMake_WithMakeCode_ThrowMakeDoesntExist()
        {
            string makeCode = "HON-99999999";
            var errorMessage = $"Make code {makeCode} does not exist";

            this._MakeRepository
                .Setup(m => m.FindMakeByMakeCodeAsync(makeCode))
                .ReturnsAsync((Make?)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => this._MakeService.ActivateMake(makeCode));
            Assert.Contains(errorMessage, ex.Message);
        }

        [Fact]
        public async void CreateMakeTest()
        {
            MakeRequestBody bodyData = new MakeRequestBody()
            {
                MakeName = "Honda",
                MakeAbbreviation = "HON",
                MakeLogo = "",
                Origin = "Japan",
                Company = "Honda Ltda",
                YearFounded = 1950
            };

            this._MakeRepository.Setup(m => m.FindMakeByMakeNameAsync(bodyData.MakeName)).ReturnsAsync((Make?)null);
            this._MakeRepository.Setup(m => m.FindByMakeAbbreviationAsync(bodyData.MakeAbbreviation)).ReturnsAsync((Make?)null).Verifiable();


            this._MakeRepository
                .Setup(m => m.CreateMakeAsync(It.IsAny<Make>()))
                .ReturnsAsync(true);

            var res = await this._MakeService.CreateMake(bodyData);

            Assert.True(res);
        }

        [Fact]
        async public void GetMakeTest() 
        {
            List <Make> makesMockDataList = new List<Make>() 
            { 
                new Make()
                {
                    Id = Guid.NewGuid(),
                    MakeName = "Chevrolet",
                    Active = false,
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
                    Active = false,
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

            this._MakeRepository.Setup(r => r.FindMakesAsync()).ReturnsAsync(makesMockDataList);

            var result = await this._MakeService.GetMakes();
            result.Should().BeEquivalentTo(makesMockDataList);
        }

        [Fact]
        public async void ThrowErrorTest()
        {
            MakeRequestBody bodyData = new MakeRequestBody()
            {
                MakeName = "Honda",
                MakeAbbreviation = "HON",
                MakeLogo = "",
                Origin = "Japan",
                Company = "Honda Ltda",
                YearFounded = 1950
            };

            this._MakeRepository.Setup(m => m.FindMakeByMakeNameAsync(bodyData.MakeName)).ReturnsAsync(new Make()
            {
                Id = Guid.NewGuid(),
                MakeName = "Honda",
                Active = true,
                MakeAbbreviation = "HON",
                MakeLogo = "",
                Company = "Japan Ltda",
                Models = [],
                DateCreated = DateTime.Now,
                MakeCode = "CHE-12345678",
                Origin = "USA",
                YearFounded = 1920
            });


          var ex = await Assert.ThrowsAsync<ArgumentException>(()=> this._MakeService.CreateMake(bodyData));
            Assert.Contains($"{bodyData.MakeName} already exsits", ex.Message);
        }

        [Fact]
        public async Task CreateMultipleMakes()
        {
            string? projectDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            string?  filePath = Path.Combine(projectDir, "TestFiles", "test.xlsx");
            using var stream = File.OpenRead(filePath);


            IFormFile file = new FormFile(stream, 0, stream.Length, "FileData", "test.xlsx");

            CreateMultipleMakeDto body = new CreateMultipleMakeDto()
            {
                FileData = file
            };

            this._MakeRepository
                .Setup(m => m.FindMakeByMakeNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Make?)null);

            this._MakeRepository
                .Setup(m => m.FindByMakeAbbreviationAsync(It.IsAny<string>()))
                .ReturnsAsync((Make?)null);

            this._MakeRepository
                .Setup(m => m.CreateMultipleMakesAsync(It.IsAny<List<Make>>()))
                .ReturnsAsync(true);

            var result = await this._MakeService.CreateMultipleMakes(body);

            Assert.Equal("Transaction Successful", result);
        }

        [Fact]
        public void DownloadMakesTemplateTest()
        {
            var result = this._MakeService.DownloadMakesTemplate();

            Assert.IsType<byte[]>(result);
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            using var stream = new MemoryStream(result);
            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet("Makes");

            Assert.Equal("Make Name", worksheet.Cell(1, 1).GetString());
            Assert.Equal("Origin", worksheet.Cell(1, 2).GetString());
            Assert.Equal("Compnay", worksheet.Cell(1, 3).GetString());
            Assert.Equal("Year Founded", worksheet.Cell(1, 4).GetString());
            Assert.Equal("Make Abbreviation", worksheet.Cell(1, 5).GetString());
        }

        [Fact]
        public async Task GetMakeByMakeNameTest()
        {
            Make makeData = CreateMake();
            string makeName = "Chevrolet";

            this._MakeRepository
                .Setup(m=>m.FindMakeByMakeNameAsync(makeName))
                .ReturnsAsync(makeData);

            var result = await this._MakeService.GetMakeByMakeName(makeName);

            Assert.IsType<Make>(result);
            result.Should().BeEquivalentTo(makeData);
        }

        [Fact]
        public async Task GetMakesNameListTest()
        {
            List<MakeNameList> mockListData = new List<MakeNameList>()
            {
                new MakeNameList
                {
                    MakeName = "Honda",
                    MakeCode = "HON-12345678"
                },
                new MakeNameList
                {
                    MakeName = "Chevrolet",
                    MakeCode = "CHE-12345678"
                }
            };
            this._MakeRepository
                .Setup(m => m.GetMakeNameListAsync())
                .ReturnsAsync(mockListData);

            var res = await this._MakeService.GetMakeNameListAsync();

            Assert.IsType<List<MakeNameList>>(res);
            res.Should().BeEquivalentTo(mockListData);
        }
    }
}