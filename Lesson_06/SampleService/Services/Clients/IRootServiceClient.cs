using RootServiceNamespace;

namespace SampleService.Services.Clients
{
    public interface IRootServiceClient
    {
        public RootServiceClient RootServiceClient { get; }
        public Task<IEnumerable<WeatherForecast>> Get();
    }
}
