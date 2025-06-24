using Domain.Entities.AdClickSystem.Domain.Entities;

namespace Application.Adapters
{
    /// <summary>
    /// Defines a generic ad client interface.
    /// This abstraction lets us plug in different strategies for making calls
    /// to the ad service (e.g., HTTP, gRPC, or direct calls).
    /// </summary>
    public interface IAdClient
    {
        /// <summary>
        /// Asynchronously gets an advertisement based on filter parameters.
        /// </summary>
        Task<Ad> GetAdAsync(string language, string country, string adSize);

        /// <summary>
        /// Asynchronously records a click for the specified advertisement.
        /// </summary>
        Task ClickAdAsync(Guid adId);

        /// <summary>
        /// Asynchronously records a registration for the specified advertisement.
        /// </summary>
        Task RegisterAdAsync(Guid adId);
    }
}
