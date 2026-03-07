using BrandMicroservice.src.Data;
using BrandMicroservice.src.Models.DbModels;
using BrandMicroservice.src.Models.Dtos.Make;
using BrandMicroservice.src.Repository;
using BrandMicroservice.src.Repository.Interfaces;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMicroservice.Test.Repositories
{
    public class MakeRepositoryTests
    {
        public MakeRepositoryTests()
        {
            
        }
        private DatabaseContext GetMockDbContext()
        {
            //var options = new DbContextOptionsBuilder<DatabaseContext>()
            //    .UseInMemoryDatabase(Guid.NewGuid().ToString())
            //    .Options;
            //var context = new DatabaseContext(options);

            //string dbPath = string.Format($"TestDatabase_{Guid.NewGuid()}.db"); // Will be in bin folder

            var connection = new SqliteConnection($"DataSource=:memory:");
            connection.Open();


            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(connection)
                .Options;

            var context = new DatabaseContext(options);

            //string connection = $"User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=brand_database_{Guid.NewGuid()};";

            //var options = new DbContextOptionsBuilder<DatabaseContext>()
            //    .UseNpgsql(connection)
            //    .Options;

            //  var context = new DatabaseContext(options);

            context.Database.EnsureCreated();

            context.Makes.AddRange(
                new Make
                {
                    Id = Guid.NewGuid(),
                    MakeName = "Chevrolet",
                    Active = true,
                    MakeAbbreviation = "CHE",
                    MakeLogo = "",
                    Company = "General Motors",
                    Models = [],
                    DateCreated = DateTime.UtcNow,
                    MakeCode = "CHE-12345678",
                    Origin = "USA",
                    YearFounded = 1920
                },
                new Make
                {
                    Id = Guid.NewGuid(),
                    MakeName = "Honda",
                    Active = true,
                    MakeAbbreviation = "Hon",
                    MakeLogo = "",
                    Company = "General Motors",
                    Models = [],
                    DateCreated = DateTime.UtcNow,
                    MakeCode = "HON-12345888",
                    Origin = "Japan",
                    YearFounded = 1920
                }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task FindByMakeCode_ReturnMake()
        {
            var context = GetMockDbContext();

            var makeRepository = new MakeRepository(context);

            var result = await makeRepository.FindMakeByMakeCodeAsync("CHE-12345678");

            Assert.NotNull(result);

            Assert.IsAssignableFrom<Make>(result);

            Assert.Equal("Chevrolet", result.MakeName);
        }

        [Fact]
        public async Task CreateMake_MakeData_ReturnTrue()
        {
            var newMake = new Make()
            {
                Id = Guid.NewGuid(),
                MakeName = "Chevrolet2",
                Active = true,
                MakeAbbreviation = "CH2",
                MakeLogo = "",
                Company = "General Motors",
                Models = [],
                DateCreated = DateTime.UtcNow,
                MakeCode = "CHE-12345655",
                Origin = "USA",
                YearFounded = 1920
            };
            var context = GetMockDbContext();

            var makeRepository = new MakeRepository(context);

            var res = await makeRepository.CreateMakeAsync(newMake);

            Assert.True(res);
        }

        [Fact]
        public async Task ActivateMake_WithMakeCode_ReturnTrue()
        {
            var context = GetMockDbContext();
            var makeRepository = new MakeRepository(context);

            var res = await makeRepository.ActivateMakeAsync("CHE-12345678");

            Assert.True(res);
        }

        [Fact]
        public async Task GetMakeByMakeNameTest()
        {
            var context = GetMockDbContext();
            var makeRepository = new MakeRepository(context);
            string makeName = "Chevrolet";
                        
            var res = await makeRepository.FindMakeByMakeNameAsync(makeName);

            Assert.IsType<Make>(res);
        }

        [Fact]
        public async Task GetMakesNameListTest()
        {
            var mockData = new List<MakeNameList>
            { 
                new MakeNameList() { MakeCode = "CHE-12345678", MakeName = "Chevrolet"},
                new MakeNameList() { MakeCode = "HON-12345888", MakeName = "Honda"}
            };
            var context = GetMockDbContext();

            var makeRepository = new MakeRepository(context);

            var res = await makeRepository.GetMakeNameListAsync();

            Assert.IsType<List<MakeNameList>>(res);
            res.Should().BeEquivalentTo(mockData);

            
        }
    }
}
