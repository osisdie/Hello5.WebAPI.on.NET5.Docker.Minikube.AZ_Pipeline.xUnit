using System.ComponentModel.DataAnnotations;

namespace CoreFX.Abstractions.Enums
{
    /// <summary>
    /// Platform Error Code
    /// </summary>
    public enum SvcCodeEnum
    {
        [Display(Name = "unset")]
        UnSet = -0001,
        [Display(Name = "error")]
        Error = 0000,
        [Display(Name = "success")]
        Success = 0001,
        [Display(Name = "exception")]
        Exception = 0003,

        ErrorCode_Max = 1000
    }
}
