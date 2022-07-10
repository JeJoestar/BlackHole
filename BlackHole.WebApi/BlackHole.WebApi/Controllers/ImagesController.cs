using BlackHole.DAL;
using BlackHole.WebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlackHole.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/images")]
    public class ImagesController : Controller
    {
        private readonly DataRepository<Image> _imageRepository;

        public ImagesController()
        {
            _imageRepository = new DataRepository<Image>(new BHDataContext());
        }

        [HttpPost("create-image")]
        public async Task<IActionResult> CreateImage([FromBody] ImageDto imageDto)
        {
            Image image = new Image()
            {
                Url = imageDto.Url,
                Name = imageDto.Name,
            };
            await _imageRepository.CreateAsync(image);

            return Ok(image);
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetImageByFilter([FromQuery]string? filter = null)
        {
            List<Image> result;
            if (string.IsNullOrEmpty(filter))
            {
                result = await _imageRepository.GetAll();
            }
            else
            {
                result = await _imageRepository.GetByFilter(i => i.Name.ToLower().Contains(filter.ToLower()));
            }

            return Ok(result);
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetImageById([FromQuery] int id)
        {
            var image = (await _imageRepository.GetAll()).FirstOrDefault(i => i.Id == id);
            if (image == null)
            {
                return BadRequest("Image id is invalid.");
            }

            return Ok(image);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteImage([FromQuery]int id)
        {
            var image = (await _imageRepository.GetAll()).FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                return BadRequest("Not found");
            }

            await _imageRepository.Delete(image);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateImage([FromQuery]int id, [FromBody]ImageDto imageDto)
        {
            var image = (await _imageRepository.GetAll()).FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                return BadRequest("Not found");
            }
            image.Url = imageDto.Url;
            image.Name = imageDto.Name;
            await _imageRepository.UpdateAsync(image);

            return Ok(image);
        }
    }
}
