using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public class IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}