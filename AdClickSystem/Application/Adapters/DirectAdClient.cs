using Domain.Entities.AdClickSystem.Domain.Entities;

namespace Application.Adapters
{
    public class DirectAdClient : IAdClient
    {
        public Task ClickAdAsync(Guid adId)
        {
            return Task.CompletedTask;
        }

        public Task<Ad> GetAdAsync(string language, string country, string adSize)
        {
            return Task.FromResult(new Ad { });
        }

        public Task RegisterAdAsync(Guid adId)
        {
            return Task.CompletedTask;
        }
    }
}
