using Application.Services;
using Serilog;

namespace MockClient
{
    public class BackgroundAdGenerator : IDisposable
    {
        private readonly IAdService _adService;
        private readonly ILogger _logger;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public BackgroundAdGenerator(IAdService adService, ILogger logger)
        {
            _adService = adService;
            _logger = logger;
        }

        /// <summary>
        /// Each 5 seconds it updates the optimized and random ad data of specific subset
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="country"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task GenerateDataAsync(string lang, string country, string size)
        {
            _logger.Information("BackgroundAdGenerator: Generating data started");
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                _adService.UpdateOptimizedAndRandomAd(lang, country, size);
                await Task.Delay(TimeSpan.FromSeconds(5), _tokenSource.Token);
            }
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
        }
    }
}
