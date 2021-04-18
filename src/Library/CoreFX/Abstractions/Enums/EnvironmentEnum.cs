using System.ComponentModel.DataAnnotations;

namespace CoreFX.Abstractions.Enums
{
    public enum EnvironmentEnum
    {
        [Display(Name = "Debug")]
        Default = 0,

        [Display(Name = "Debug")]
        Debug = 1,

        [Display(Name = "Development")]
        Development = 2,

        [Display(Name = "Testing")]
        Testing = 3,

        [Display(Name = "Staging")]
        Staging = 4,

        [Display(Name = "Production")]
        Production = 5,

        [Display(Name = "unknown")]
        Max = 255
    }

}
