using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class PhotoController : BaseAPiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PhotoController(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
            _mapper = mapper;
            _context = context;
        }

        public string GetUserById() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false
            };

            user.Photos.Add(photo);

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return Ok(_mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem addding photo");
        }


        [HttpPost("add-main-photo")]
        public async Task<ActionResult<PhotoDto>> AddMainPhoto(IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = true
            };

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            user.Photos.Add(photo);

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return Ok(_mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem addding photo");
        }


        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }
    }
}

