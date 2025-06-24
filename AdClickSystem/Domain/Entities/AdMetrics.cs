namespace Domain.Entities
{
    /// <summary>
    /// Stores advertisement-related performance metrics.
    /// </summary>
    public class AdMetrics
    {
        /// <summary>
        /// Gets or sets the total number of impressions.
        /// </summary>
        public int TotalImpressions { get; set; }

        /// <summary>
        /// Gets or sets the total number of clicks.
        /// </summary>
        public int TotalClicks { get; set; }

        /// <summary>
        /// Gets or sets the total number of registrations.
        /// </summary>
        public int TotalRegistrations { get; set; }

        /// <summary>
        /// Returns the optimization metric defined as the ratio of total registrations to total impressions.
        /// Returns 0 if there are no impressions.
        /// </summary>
        public double OptimizationMetric => TotalImpressions == 0 ? 0 : (double)TotalRegistrations / TotalImpressions;
    }
}
