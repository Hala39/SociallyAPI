using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
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
        private readonly IUserService _userService;
        private readonly IPhotoRepo _repo;
        public PhotoController(DataContext context, IMapper mapper, IUserService userService, IPhotoService photoService, IPhotoRepo repo)
        {
            _repo = repo;
            _userService = userService;
            _photoService = photoService;
            _mapper = mapper;
            _context = context;
        }

        
        [HttpGet("feed")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _repo.GetPosts();
            return Ok(posts);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false,
                IsCover = false
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = true,
                IsCover = false
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");
            if (photo.IsCover) return BadRequest("This is already your cover photo");

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

        [HttpPost("add-cover-photo")]
        public async Task<ActionResult<PhotoDto>> AddCoverPhoto(IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false,
                IsCover = true
            };

            var currentCover = user.Photos.FirstOrDefault(x => x.IsCover);

            if (currentCover != null) currentCover.IsCover = false;

            photo.IsCover = true;

            user.Photos.Add(photo);

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return Ok(_mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-cover-photo/{photoId}")]
        public async Task<ActionResult> SetCoverPhoto(int photoId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsCover) return BadRequest("This is already your cover photo");
            if (photo.IsMain) return BadRequest("This is your profile picture");

            var currentCover = user.Photos.FirstOrDefault(x => x.IsCover);

            if (currentCover != null) currentCover.IsCover = false;

            photo.IsCover = true;

            var isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved)
            {
                return NoContent();
            }

            return BadRequest("Failed to set cover photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");
            if (photo.IsCover) return BadRequest("You cannot delete your cover photo");

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

