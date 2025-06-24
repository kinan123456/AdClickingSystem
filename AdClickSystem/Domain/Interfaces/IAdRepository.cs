using Domain.Entities.AdClickSystem.Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Provides access to advertisement data.
    /// </summary>
    public interface IAdRepository
    {
        /// <summary>
        /// Gets all ads.
        /// </summary>
        IEnumerable<Ad> GetAllAds();

        /// <summary>
        /// Gets all ads matching the specified filter.
        /// </summary>
        IEnumerable<Ad> GetAdsByFilter(string language, string country, string adSize);

        /// <summary>
        /// Gets an ad by its unique identifier.
        /// </summary>
        Ad? GetAdById(System.Guid adId);

        /// <summary>
        /// Increments the impression count for the specified ad.
        /// </summary>
        void IncrementImpression(System.Guid adId);

        /// <summary>
        /// Increments the click count for the specified ad.
        /// </summary>
        void IncrementClick(System.Guid adId);

        /// <summary>
        /// Increments the registration count for the specified ad.
        /// </summary>
        void IncrementRegistration(System.Guid adId);
    }
}
