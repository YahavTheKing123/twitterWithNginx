using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Logic.Interfaces;

namespace TwitterPoc.Logic.Services
{
    public class UsersService : IUsersService
    {
        private readonly IFollowersRepository _followersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger _logger;

        public UsersService(IUsersRepository usersRepository, IFollowersRepository followersRepository, ILogger<UsersService> logger)
        {
            _usersRepository = usersRepository;
            _followersRepository = followersRepository;
            _logger = logger;
        }

        public async Task RegisterAsync(string username, string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hash = BCrypt.Net.BCrypt.HashPassword(password, salt);
            await _usersRepository.AddAsync(new User() { Username = username, Password = hash, PasswordSalt = salt });
        }

        public async Task<Entities.User> GetVerifiedUserAsync(string username, string password)
        {
            var user = await _usersRepository.GetAsync(username);
            if (user == null)
            {
                _logger.LogInformation($"User {username} does not exist");
                return null;
            }
            else if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogInformation($"User {username} has been verified.");
                return new Entities.User() { Username = user.Username };
            }
            else
            {
                _logger.LogInformation($"User {username} exists but has not been verified with the supplied password.");
                return null;
            }

        }

        public async Task AddFollower(string follower, string followee)
        {
            await _followersRepository.Add(follower, followee);
        }

        public async Task RemoveFollower(string follower, string followee)
        {
            await _followersRepository.Remove(follower, followee);
        }
    }
}
