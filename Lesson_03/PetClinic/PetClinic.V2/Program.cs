using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetClinic.Data;
using PetClinic.V2.Services.Impl;
using System.Net;

namespace PetClinic.V2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Дополнительная конфигурация для работы gRPC
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5100, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    //listenOptions.UseHttps(@"c:\gbtestcert.pfx", "12345");
                });
                options.Listen(IPAddress.Any, 5101, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    //listenOptions.UseHttps(@"c:\gbtestcert.pfx", "12345");
                });
            });

            // Добавляем поддержку работы с gRPC
            // Добавить пакет Microsoft.AspNetCore.Grpc.JsonTranscoding
            builder.Services.AddGrpc().AddJsonTranscoding();


            // Add DbContext
            builder.Services.AddDbContext<PetClinicDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });


            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Configure Swagger

            // https://learn.microsoft.com/ru-ru/ASPNET/Core/grpc/json-transcoding-openapi?view=aspnetcore-8.0

            builder.Services.AddGrpcSwagger();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "Pet Clinic Service", Version = "v1" });

                // Считывание файла с автоматически сыормированной документацией из прото файла
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "PetClinic.V2.xml");
                c.IncludeXmlComments(filePath);
                c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Clinic Service API V1");
                });
            }

            app.UseRouting();
            app.UseAuthorization();

            //app.MapControllers();

            // Установить
            // Microsoft.AspNetCore.Grpc.Swagger
            // Google.Api.CommonProtos
            // Grpc.AspNetCore.Web

            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            app.MapGrpcService<ClinicService>().EnableGrpcWeb();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made throught a gRPC client.");

            app.Run();
        }
    }
}