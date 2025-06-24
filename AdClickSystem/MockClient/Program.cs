using Serilog;

internal class Program
{
    private static readonly Random RandomGenerator = new Random();

    public static async Task Main(string[] args)
    {
        // Configure Serilog.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        Log.Information("MockClient started using DirectAdClient adapter.");

        while (true)
        {
            try
            {
                // Get an ad using the adapter.
                Log.Information("MockClient: GetAdAsync returned AdId: {AdId}, Title: {Title}", 5, 5);

                // Simulate a click with 50% chance.
                if (RandomGenerator.NextDouble() < 0.5)
                {
                    Log.Information("MockClient: ClickAdAsync executed for AdId: {AdId}", 5);
                }

                // Simulate a registration with 30% chance.
                if (RandomGenerator.NextDouble() < 0.3)
                {
                    Log.Information("MockClient: RegisterAdAsync executed for AdId: {AdId}", 5);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "MockClient: Error during ad operations");
            }

            // Wait for a random interval between 2 and 5 seconds.
            int delayMilliseconds = RandomGenerator.Next(2000, 5000);
            await Task.Delay(delayMilliseconds);
        }
    }
}