namespace Domain.Entities
{
    using System;

    namespace AdClickSystem.Domain.Entities
    {
        /// <summary>
        /// Represents an advertisement with its metadata.
        /// </summary>
        public class Ad
        {
            /// <summary>
            /// Gets or sets the unique identifier of the ad.
            /// </summary>
            public Guid AdId { get; set; } = Guid.NewGuid();

            /// <summary>
            /// Gets or sets the title of the ad.
            /// </summary>
            public string Title { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the description of the ad.
            /// </summary>
            public string Description { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the media URL for the ad.
            /// </summary>
            public string MediaUrl { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the language of the ad.
            /// </summary>
            public string Language { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the target country for the ad.
            /// </summary>
            public string Country { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the ad size (e.g., M, L).
            /// </summary>
            public string AdSize { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the performance metrics for the ad.
            /// </summary>
            public AdMetrics Metrics { get; set; } = new AdMetrics();
        }
    }
}