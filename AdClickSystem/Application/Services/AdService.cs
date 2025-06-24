using Domain.Entities.AdClickSystem.Domain.Entities;
using Domain.Interfaces;
using Serilog;
using System.Collections.Concurrent;

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
        private readonly ConcurrentDictionary<string, Ad> _optimizedAdCache = new();

        // Cache for randomized ads keyed by "language-country-adSize"
        private readonly ConcurrentDictionary<string, Ad> _randomizedAdCache = new();

        public AdService(IAdRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Ad GetAd(string language, string country, string adSize)
        {
            var cacheKey = GetCacheKey(language, country, adSize);
            Ad? selectedAd = null;

            // 50% Chance to get optimized ad from optimized ad cache.
            if (_random.NextDouble() < 0.5)
            {
                _logger.Information("Optimized Ad Selected!");
                _optimizedAdCache.TryGetValue(cacheKey, out selectedAd);
            }
            // 50% Chance to get randomized ad from optimized ad cache.
            else
            {
                _logger.Information("Random Ad Selected!");
                _randomizedAdCache.TryGetValue(cacheKey, out selectedAd);
            }

            if (selectedAd == null)
                throw new Exception("Cached ad not found. Make sure background job is updating the cache.");

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

        public void UpdateOptimizedAndRandomAd(string lang, string country, string size)
        {
            var cacheKey = GetCacheKey(lang, country, size);
            var ads = _repository.GetAdsByFilter(lang, country, size).ToList();

            if (!ads.Any()) return;

            var optimizedAd = CalculateOptimizedAd(ads);
            var randomizedAd = ads[_random.Next(ads.Count)];

            _optimizedAdCache[cacheKey] = optimizedAd;
            _randomizedAdCache[cacheKey] = randomizedAd;

            _logger.Warning("Updated caches for key {CacheKey}: OptimizedAd={OptimizedId}, RandomAd={RandomId}",
                cacheKey, optimizedAd.AdId, randomizedAd.AdId);
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
