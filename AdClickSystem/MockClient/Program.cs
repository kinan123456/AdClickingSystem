using Application.Adapters;
using Application.Repositories;
using Application.Services;
using Domain.Interfaces;
using MockClient;
using Serilog;

internal class Program
{
    private static readonly Random RandomGenerator = new Random();

    public static async Task Main(string[] args)
    {
        // Configure Serilog.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        Log.Information("MockClient started using DirectAdClient adapter.");

        // Set up dependencies manually.
        IAdRepository adRepository = new InMemoryAdRepository(); // IAdRepository is the Database interface. Initiate In-Memory database class.
        IAdService adService = new AdService(adRepository, Log.Logger); // IAdService is the logic that accesses the database collections.
        IAdClient adClient = new DirectAdClient(adService, Log.Logger); // Create the adapter instance (DirectAdClient).

        string lang = "HE";
        string country = "ISRAEL";
        string size = "M";
        adService.UpdateOptimizedAndRandomAd(lang, country, size);

        BackgroundAdGenerator generator = new BackgroundAdGenerator(adService, Log.Logger);
        _ = Task.Run(async () => await generator.GenerateDataAsync(lang, country, size));

        while (true)
        {
            try
            {
                Log.Information("MockClient: Request GetAd with details: Language = {Language}, Country = {Country}, Size = {Size}",
                    lang, country, size);

                // Get an ad using the adapter.
                var ad = await adClient.GetAdAsync(lang, country, size);
                Log.Information("MockClient: GetAdAsync returned AdId: {AdId}, Title: {Title}", ad.AdId, ad.Title);

                // Simulate a click with 50% chance.
                if (RandomGenerator.NextDouble() < 0.5)
                {
                    Log.Information("MockClient: ClickAdAsync executed for AdId: {AdId}", ad.AdId);
                    await adClient.ClickAdAsync(ad.AdId);

                    // Simulate a registration with 30% chance.
                    if (RandomGenerator.NextDouble() < 0.3)
                    {
                        Log.Information("MockClient: RegisterAdAsync executed for AdId: {AdId}", ad.AdId);
                        await adClient.RegisterAdAsync(ad.AdId);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "MockClient: Error during ad operations");
            }

            // Wait for a 10 milliseconds for the next client call.
            var delayMilliseconds = 10;
            await Task.Delay(delayMilliseconds);
        }
    }
}