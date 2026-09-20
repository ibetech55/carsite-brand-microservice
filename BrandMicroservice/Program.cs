
using BrandMicroservice.src.Data;
using BrandMicroservice.src.Mapping;
using BrandMicroservice.src.Middlewares.GlobalException;
using BrandMicroservice.src.Repository;
using BrandMicroservice.src.Repository.Interfaces;
using BrandMicroservice.src.Services;
using BrandMicroservice.src.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BrandMicroservice
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BrandMSCors", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


            builder.Services.AddScoped<IMakeService, MakeService>();
            builder.Services.AddScoped<IModelService, ModelService>();

            builder.Services.AddScoped<IMakeRepository, MakeRepository>();
            builder.Services.AddScoped<IModelRepository, ModelRepository>();
            builder.Services.AddHttpClient();


            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Mapping>();
            });

            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsProduction())
            {
                // AWS SSM PARAMETER STORE
                builder.Configuration.AddSystemsManager("/carsite/dev");
            }



            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                var dbString = builder.Configuration["BRAND_DATABASE"];
                options.UseNpgsql(dbString);
                Console.WriteLine($"Connected to DB on {dbString}");
            });

            var app = builder.Build();

            app.UseCors("BrandMSCors");

            app.UseMiddleware<GlobalExcepetionMiddleware>();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            //        c.RoutePrefix = string.Empty;
            //    });
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            app.MapControllers();

            app.Run();
        }
    }
}