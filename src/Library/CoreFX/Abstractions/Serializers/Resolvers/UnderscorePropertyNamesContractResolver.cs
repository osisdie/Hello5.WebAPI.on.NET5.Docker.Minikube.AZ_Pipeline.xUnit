using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace CoreFX.Abstractions.Serializers.Resolvers
{
    public class UnderscorePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName) =>
            Regex.Replace(propertyName, @"(\w)([A-Z])", "$1_$2").ToLower();
    }
}
