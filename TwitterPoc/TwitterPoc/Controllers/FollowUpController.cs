using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Logic.Interfaces;

namespace TwitterPoc.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FollowUpController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public FollowUpController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Route("Follow")]
        [HttpPost]
        public IActionResult Follow(string username)
        {
            var follower = User.Identity.Name;
            _usersService.AddFollower(follower, username);
            return Ok();
        }

        [Route("Unfollow")]
        [HttpPost]
        public IActionResult Unfollow(string username)
        {
            var follower = User.Identity.Name;
            _usersService.RemoveFollower(follower, username);
            return Ok();
        }
    }
    
}
