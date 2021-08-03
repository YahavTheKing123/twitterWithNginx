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
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("PostMessage - invalid message: " + model?.MessageBody);
                return BadRequest(ModelState);
            }
            await _feedsService.AddMessage(User.Identity.Name, model.MessageBody);
            return Ok();
        }

        [Route("GetFeed")]
        [HttpGet]
        public async Task<IActionResult> GetFeed(string username)
        {
            return await GetFeedModel(username, true);
        }

        [Route("GetMyFeed")]
        [HttpGet]
        public async Task<IActionResult> GetMyFeed(string username)
        {
            return await GetFeedModel(username, false);
        }

        private async Task<IActionResult> GetFeedModel(string username, bool isGloablFeed)
        {
            FeedModel feedModel;
            if (string.IsNullOrEmpty(username))
            {
                feedModel = new FeedModel() { Messages = Enumerable.Empty<MessageModel>() };
            }
            else
            {
                var getFeedTask = isGloablFeed ?
                    _feedsService.GetGlobalFeed(username) :
                    _feedsService.GetUserFeed(User.Identity.Name, username);

                feedModel = new FeedModel(await getFeedTask);
            }
            return Ok(feedModel);
        }

    }
    
}
