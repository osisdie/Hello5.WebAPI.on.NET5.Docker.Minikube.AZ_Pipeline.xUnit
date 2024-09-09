using CoreFX.Common;
using Hello8.Domain.Common.Models;

namespace Hello8.Domain.Common
{
    public sealed class HelloContext : SvcContext
    {
        public static HelloConfiguration Settings = new HelloConfiguration();
    }
}
