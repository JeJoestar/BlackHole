using System.ComponentModel.DataAnnotations;

namespace BlackHole.WebApi.Dto
{
    public class SignUpUserDto
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        public string RepeatPassword { get; set; }
    }
}
