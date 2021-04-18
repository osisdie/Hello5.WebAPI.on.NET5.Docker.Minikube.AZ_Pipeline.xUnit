using System.ComponentModel.DataAnnotations;

namespace Hello5.Domain.Contract.Models.Echo
{
    public class HelloEchoRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Send { get; set; }
    }
}
