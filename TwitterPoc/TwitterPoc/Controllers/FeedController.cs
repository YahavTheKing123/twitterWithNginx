using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Models;

namespace TwitterPoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedController : ControllerBase
    {
        [Route("Follow")]
        [HttpPost]
        public IActionResult PostMessage([FromBody]PostedMessageModel model)
        {
            return Ok();
        }

        [Route("GetFeed")]
        [HttpGet]
        public IActionResult GetFeed(string username)
        {
            return Ok();
        }

        [Route("GetMyFeed")]
        [HttpGet]
        public IActionResult GetMyFeed(string username)
        {
            return Ok();
        }

    }
}
