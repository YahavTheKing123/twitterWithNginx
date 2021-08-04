using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterPoc.Data.Exceptions;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Data.Repositories;
using TwitterPoc.Data.Settings;
using TwitterPoc.Logic;
using TwitterPoc.Logic.Interfaces;
using TwitterPoc.Models;

namespace TwitterPoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IUsersService _usersService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IConfiguration config, ITokenService tokenService, IUsersService usersService, ILogger<AuthenticationController> logger)
        {
            _tokenService = tokenService;
            _usersService = usersService;
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterModel userModel)
        {
            _logger.LogInformation($"SignUp request. Username: {userModel?.Username}");
            if (!ModelState.IsValid)
            {
                var modelErrors = JsonConvert.SerializeObject(ModelState.Values.Select(v => v.Errors.FirstOrDefault()));
                _logger.LogInformation($"SignUp BadRequest. Errors: {modelErrors}");
                return BadRequest(ModelState);
            }
            try
            {
                await _usersService.RegisterAsync(userModel.Username, userModel.Password);
                _logger.LogInformation($"SignUp - Username: {userModel?.Username}. Successfully registered .");
            }
            catch (UsernameAlreadyExistsException e)
            {
                var message = $"Username '{e.Username}' is not available.";
                _logger.LogInformation($"SignUp - {message}");
                return Ok(new ResponseModel(false, $"Username '{e.Username}' is not available."));
            }

            return Ok(new ResponseModel(true, string.Empty));
        }

        [AllowAnonymous]
        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel userModel)
        {
            _logger.LogInformation($"SignIn request. Username: {userModel?.Username}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var verifiedUser = await _usersService.GetVerifiedUserAsync(userModel.Username, userModel.Password);

            if (verifiedUser != null)
            {
                var generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), verifiedUser);
                Response.Cookies.Append("X-Access-Token", generatedToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
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


        [Route("SignOut")]
        [HttpPost]
        public new IActionResult SignOut()
        {
            Response.Cookies.Delete("X-Access-Token");
            return Ok();
        }



    }
}
