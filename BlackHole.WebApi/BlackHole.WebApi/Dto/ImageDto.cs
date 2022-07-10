using System.ComponentModel.DataAnnotations;

namespace BlackHole.WebApi.Dto
{
    public class ImageDto
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
