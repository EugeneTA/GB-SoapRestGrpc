using Microsoft.AspNetCore.Mvc;
using RootServiceNamespace;
using SampleService.Services.Clients;

namespace SampleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly RootServiceClient _rootServiceClient;
        private readonly IRootServiceClient _rootServiceClient;

        //public WeatherForecastController(
        //    ILogger<WeatherForecastController> logger,
        //    IHttpClientFactory httpClientFactory)
        //{
        //    _logger = logger;
        //    //_httpClientFactory = httpClientFactory;
        //    _rootServiceClient = new RootServiceClient
        //        (
        //        "http://localhost:5145",
        //        httpClientFactory.CreateClient("RootServiceClient")
        //        );
        //}

        public WeatherForecastController(
                ILogger<WeatherForecastController> logger,
                IRootServiceClient rootServiceClient)
        {
            _logger = logger;
            _rootServiceClient = rootServiceClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get() => await _rootServiceClient.Get();

    }
}