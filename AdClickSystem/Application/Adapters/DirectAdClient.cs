using Application.Services;
using Domain.Entities.AdClickSystem.Domain.Entities;
using Serilog;

namespace Application.Adapters
{
    /// <summary>
    /// Direct implementation of an ad client adapter.
    /// This class calls the IAdService methods directly.
    /// If you later switch to an HTTP or gRPC adapter, simply implement IAdClient accordingly.
    /// </summary>
    public class DirectAdClient : IAdClient
    {
        private readonly IAdService _adService;
        private readonly ILogger _logger;

        public DirectAdClient(IAdService adService, ILogger logger)
        {
            _adService = adService;
            _logger = logger;
        }

        public async Task ClickAdAsync(Guid adId)
        {
            _adService.ClickAd(adId);
            _logger.Information("DirectAdClient: ClickAdAsync recorded click for ad {AdId}", adId);
            await Task.CompletedTask;
        }

        public async Task<Ad> GetAdAsync(string language, string country, string adSize)
        {
            var ad = _adService.GetAd(language, country, adSize);
            _logger.Information("DirectAdClient: GetAdAsync returned ad {AdId} for {Language}-{Country}-{AdSize}",
                ad.AdId, language, country, adSize);
            return await Task.FromResult(ad);
        }

        public async Task RegisterAdAsync(Guid adId)
        {
            _adService.RegisterAd(adId);
            _logger.Information("DirectAdClient: RegisterAdAsync recorded registration for ad {AdId}", adId);
            await Task.CompletedTask;
        }
    }
}
