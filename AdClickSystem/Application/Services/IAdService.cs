using Domain.Entities.AdClickSystem.Domain.Entities;

namespace Application.Services
{
    /// <summary>
    /// Defines ad-related business operations.
    /// </summary>
    public interface IAdService
    {
        /// <summary>
        /// Gets an ad based on the specified parameters.
        /// Increases the ad’s impression count.
        /// Returns 50% of the time the most optimized ad (by registration/impression ratio)
        /// and 50% of the time a random ad from the available ads.
        /// </summary>
        Ad GetAd(string language, string country, string adSize);

        /// <summary>
        /// Records a click against a specified ad.
        /// </summary>
        void ClickAd(Guid adId);

        /// <summary>
        /// Records a registration against a specified ad.
        /// </summary>
        void RegisterAd(Guid adId);
    }
}
