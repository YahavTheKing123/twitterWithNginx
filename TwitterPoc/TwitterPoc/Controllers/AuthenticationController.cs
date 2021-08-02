using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Logic;
using TwitterPoc.Models;

namespace TwitterPoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IConfiguration config, ITokenService tokenService, IUsersRepository usersRepository, ILogger<AuthenticationController> logger)
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel userModel)
        {
            _logger.LogInformation("SignIn request.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validUser = await _usersRepository.GetUserAsync(userModel.Username, userModel.Password);

            if (validUser != null)
            {
                var generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
                if (generatedToken != null)
                {
                    return Ok(new ResponseModel(true, generatedToken));
                }
                else
                {
                    _logger.LogError("Could not create token.");

                    return Problem("Error. Please contact admin");
                }
            }
            else
            {
                return Ok(new ResponseModel(false, "Username or password are not correct"));
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("SignOut")]
        [HttpPost]
        public string SignOut()
        {
        }



    }
}
