using Domain.Entities.AdClickSystem.Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Application.Repositories
{
    /// <summary>
    /// An in-memory repository that stores advertisement data.
    /// </summary>
    public class InMemoryAdRepository : IAdRepository
    {
        private readonly ConcurrentDictionary<Guid, Ad> _ads;

        public InMemoryAdRepository()
        {
            _ads = new ConcurrentDictionary<Guid, Ad>();

            FillInAdsDatabase();
        }

        private void FillInAdsDatabase()
        {
            // Seed with some sample ads.
            var sampleAds = new List<Ad>
            {
                new Ad {
                    AdId = Guid.NewGuid(),
                    Title = "Ad One",
                    Description = "Buy our first product",
                    MediaUrl = "first",
                    Language = "HE",
                    Country = "ISRAEL",
                    AdSize = "M"
                },
                new Ad {
                    AdId = Guid.NewGuid(),
                    Title = "Ad Two",
                    Description = "Buy our second product",
                    MediaUrl = "second",
                    Language = "HE",
                    Country = "ISRAEL",
                    AdSize = "M"
                },
            };

            foreach (var ad in sampleAds)
            {
                _ads.TryAdd(ad.AdId, ad);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Ad> GetAdsByFilter(string language, string country, string adSize)
        {
            return _ads.Values.Where(ad =>
                ad.Language.Equals(language, StringComparison.OrdinalIgnoreCase) &&
                ad.Country.Equals(country, StringComparison.OrdinalIgnoreCase) &&
                ad.AdSize.Equals(adSize, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public Ad? GetAdById(Guid adId)
        {
            _ads.TryGetValue(adId, out var ad);
            return ad;
        }

        /// <inheritdoc/>
        public void IncrementImpression(Guid adId)
        {
            if (_ads.TryGetValue(adId, out var ad))
            {
                lock (ad)
                {
                    ad.Metrics.TotalImpressions++;
                }
            }
        }

        /// <inheritdoc/>
        public void IncrementClick(Guid adId)
        {
            if (_ads.TryGetValue(adId, out var ad))
            {
                lock (ad)
                {
                    ad.Metrics.TotalClicks++;
                }
            }
        }

        /// <inheritdoc/>
        public void IncrementRegistration(Guid adId)
        {
            if (_ads.TryGetValue(adId, out var ad))
            {
                lock (ad)
                {
                    ad.Metrics.TotalRegistrations++;
                }
            }
        }
    }
}
