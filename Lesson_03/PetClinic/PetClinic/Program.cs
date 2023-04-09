using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PetClinic.Data;
using PetClinic.Services.Impl;
using System.Net;

namespace PetClinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Дополнительная конфигурация для работы gRPC
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    //listenOptions.UseHttps(@"c:\gbtestcert.pfx", "12345");
                });
            });

            // Add services to the container.

            // Добавляем поддержк уработы с gRPC
            builder.Services.AddGrpc();

            // Add DbContext
            builder.Services.AddDbContext<PetClinicDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            // Добавляем gRPC сервисы
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ClinicService>();
            });

            app.Run();
        }
    }
}