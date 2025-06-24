using Domain.Entities.AdClickSystem.Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Application.Services
{
    /// <summary>
    /// Provides advertisement-related business operations.
    /// </summary>
    public class AdService : IAdService
    {
        private readonly IAdRepository _repository;
        private readonly ILogger _logger;
        private static readonly Random _random = new();

        // Cache for optimized ads keyed by "language-country-adSize"
        private readonly Dictionary<string, Ad> _optimizedAdCache = new();

        public AdService(IAdRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Ad GetAd(string language, string country, string adSize)
        {
            var ads = _repository.GetAdsByFilter(language, country, adSize).ToList();
            if (!ads.Any())
            {
                throw new Exception("No ads available for the specified parameters.");
            }

            Ad selectedAd;
            // Randomly choose: 50% chance for the optimized ad, 50% for a random ad.
            if (_random.NextDouble() < 0.5)
            {
                var cacheKey = GetCacheKey(language, country, adSize);
                if (!_optimizedAdCache.TryGetValue(cacheKey, out var optimizedAd) || optimizedAd == null)
                {
                    optimizedAd = CalculateOptimizedAd(ads);
                    _optimizedAdCache[cacheKey] = optimizedAd;
                }
                selectedAd = optimizedAd;
            }
            else
            {
                selectedAd = ads[_random.Next(ads.Count)];
            }

            _repository.IncrementImpression(selectedAd.AdId);
            _logger.Information("GetAd: Returned ad {AdId} (Title: {Title})", selectedAd.AdId, selectedAd.Title);
            return selectedAd;
        }

        /// <inheritdoc/>
        public void ClickAd(Guid adId)
        {
            _repository.IncrementClick(adId);
            _logger.Information("ClickAd: Recorded click for ad {AdId}", adId);
        }

        /// <inheritdoc/>
        public void RegisterAd(Guid adId)
        {
            _repository.IncrementRegistration(adId);
            _logger.Information("RegisterAd: Recorded registration for ad {AdId}", adId);
        }

        private string GetCacheKey(string language, string country, string adSize) =>
            $"{language}-{country}-{adSize}";

        private Ad CalculateOptimizedAd(List<Ad> ads)
        {
            // Choose the ad with the highest optimization metric from its metrics.
            return ads.OrderByDescending(ad => ad.Metrics.OptimizationMetric).First();
        }
    }
}
