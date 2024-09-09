using System.ComponentModel.DataAnnotations;

namespace Hello8.Domain.Contract.Models.Echo
{
    public class HelloEchoRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Send { get; set; }
    }
}
