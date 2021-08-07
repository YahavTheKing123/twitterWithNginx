using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Logic.Interfaces;
using TwitterPoc.Models;

namespace TwitterPoc.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedController : ControllerBase
    {
        private readonly IFeedsService _feedsService;
        private readonly ILogger _logger;

        public FeedController(IFeedsService feedsService, ILogger<FeedController> logger)
        {
            _feedsService = feedsService;
            _logger = logger;
        }

        [Route("PostMessage")]
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody]PostedMessageModel model)
        {
            _logger.LogInformation($"PostMessage - model.MessageBody: {model.MessageBody}");
            await _feedsService.AddMessage(User.Identity.Name, model.MessageBody);
            return Ok();
        }

        [Route("GetFeed/{username?}")]
        [HttpGet]
        public async Task<IActionResult> GetFeed(string username)
        {
            _logger.LogInformation($"GetFeed - username: {username}");

            return await GetFeedModel(username, true);
        }

        [Route("GetMyFeed/{username?}")]
        [HttpGet]
        public async Task<IActionResult> GetMyFeed(string username)
        {
            return await GetFeedModel(username, false);
        }

        private async Task<IActionResult> GetFeedModel(string username, bool isGloablFeed)
        {
            var getFeedTask = isGloablFeed ?
                _feedsService.GetGlobalFeed(User.Identity.Name, username) :
                _feedsService.GetUserFeed(User.Identity.Name, username);

            var feedModel = new FeedModel(await getFeedTask);

            return Ok(feedModel);
        }

    }
    
}
