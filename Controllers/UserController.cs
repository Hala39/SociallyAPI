using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseAPiController
    {
        private readonly IUserRepo _repo;
        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            return Ok(await _repo.GetMyAccount());
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            return Ok(await _repo.GetUserByUserName(userName));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo(AppUserDto newInfo)
        {
            return Ok(await _repo.UpdateUserInfo(newInfo));
        }

    }
}