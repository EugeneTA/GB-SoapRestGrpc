using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Net.Http.Headers;
using NLog.Web;
using Polly;
using SampleService.Services.Clients;
using SampleService.Services.Clients.Impl;

namespace SampleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Logging

            builder.Services.AddHttpLogging(log =>
            {
                log.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                log.RequestBodyLogLimit = 4096;
                log.ResponseBodyLogLimit = 4096;
                log.RequestHeaders.Add(HeaderNames.Authorization);
                log.RequestHeaders.Add("X-Real-IP");
                log.RequestHeaders.Add("X-Forwarded-For");
            });

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            #endregion


            // Add services to the container.
            //builder.Services.AddHttpClient("RootServiceClient", client => 
            //{ 

            //});


            // Установить библиотеку Microsoft.Extensions.Http.Polly
            // Методы расширения из из бибилиотеки Polly.
            //
            builder.Services.AddHttpClient<IRootServiceClient, RootServiceClient>("RootServiceClient")
                .AddTransientHttpErrorPolicy
                (
                    configurePolicy => configurePolicy.WaitAndRetryAsync(
                        retryCount: 3, 
                        sleepDurationProvider: (retryCount) => TimeSpan.FromSeconds(retryCount*2),
                        onRetry: (response, sleepDuration, attemptNumber, context) =>
                        {
                            var logger = builder?.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                            logger?.LogError(response.Exception != null ? response.Exception :
                                new Exception($"\n{response.Result.StatusCode} : {response.Result.RequestMessage}"),
                                $"(attempt: {attemptNumber}) RootServiceClient request exeption");
                        }
                        )

                );

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

            app.UseAuthorization();

            // 3.1 Стандартный HTTP логгер не работает с gGRPC .Net6. Будет ошибка.
            app.UseHttpLogging();
            
            app.MapControllers();

            app.Run();
        }
    }
}