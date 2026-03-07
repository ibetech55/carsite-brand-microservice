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
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

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

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Mapping>();
            });

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                var dbString = builder.Configuration["Database"];
                options.UseNpgsql(dbString);
            });

            var app = builder.Build();

            app.UseCors("BrandMSCors");

            app.UseMiddleware<GlobalExcepetionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            //app.UseSwagger();
            //app.UseSwaggerUI(c => {
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.MapControllers();

            app.Run();
        }
    }
}