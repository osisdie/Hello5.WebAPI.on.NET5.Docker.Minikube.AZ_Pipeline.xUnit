using System;
using System.Collections.Generic;

namespace CoreFX.Abstractions.Configs
{
    public class SvcContextConfiguration
    {
        public string ASPNETCORE_ENVIRONMENT { get; set; }
        public string Version { get; set; }
        public string ApiToken { get; set; }
        public string ApiName { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, bool> FeatureToggle { get; set; } = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    }
}
