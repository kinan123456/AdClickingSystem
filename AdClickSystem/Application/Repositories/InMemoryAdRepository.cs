using Domain.Entities;
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
            var titles = new[] { "Ad A", "Ad B", "Ad C", "Ad D", "Ad E", "Ad F" };
            var descriptions = new[] { "Buy", "Sale", "Now", "Deal", "Hot", "New" };
            var mediaUrls = new[] { "url1", "url2", "url3" };

            var rand = new Random();
            string language = "HE";
            string country = "ISRAEL";
            string adSize = "M";

            for (int i = 0; i < 30; i++)
            {
                var ad = new Ad
                {
                    AdId = Guid.NewGuid(),
                    Title = titles[rand.Next(titles.Length)],
                    Description = descriptions[rand.Next(descriptions.Length)],
                    MediaUrl = mediaUrls[rand.Next(mediaUrls.Length)],
                    Language = language,
                    Country = country,
                    AdSize = adSize
                };

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
