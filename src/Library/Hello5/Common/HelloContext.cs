using CoreFX.Common;
using Hello5.Domain.Common.Models;

namespace Hello5.Domain.Common
{
    public sealed class HelloContext : SvcContext
    {
        public static HelloConfiguration Settings = new HelloConfiguration();
    }
}
