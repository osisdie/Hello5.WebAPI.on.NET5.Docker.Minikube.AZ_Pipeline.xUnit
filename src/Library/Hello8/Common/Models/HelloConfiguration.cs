namespace Hello8.Domain.Common.Models
{
    public class HelloConfiguration
    {
        /// <summary>
        /// Config relative path
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// APM for future use
        /// </summary>
        public string IsProfiling { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        public string HELLODB_CONN { get; set; }

        /// <summary>
        /// Cache connection string
        /// </summary>
        public string REDIS_CACHE_CONN { get; set; }
    }
}
