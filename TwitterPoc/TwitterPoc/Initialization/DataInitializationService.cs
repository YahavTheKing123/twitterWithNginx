using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Logic.Interfaces;

namespace TwitterPoc.Initialization
{
    public class DataInitializationService : IDataInitializationService
    {
        private readonly IUsersService _usersService;
        private readonly IFeedsService _feedsService;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger _logger;

        public DataInitializationService(IUsersService usersService, IFeedsService feedsService, IUsersRepository usersRepository, ILogger<DataInitializationService> logger)
        {
            _usersService = usersService;
            _feedsService = feedsService;
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task AddSampleDataIfEmptyProject()
        {
            _logger.LogInformation("Method AddSampleDataIfEmptyProject has been called.");
            try
            {
                var containsAnyUser = await _usersRepository.ContainsAnyUser();
                if (containsAnyUser)
                {
                    _logger.LogInformation("AddSampleDataIfEmptyProject - database already contains users. No need of sample data initialization.");
                }
                else
                {
                    _logger.LogInformation("AddSampleDataIfEmptyProject - database does not contain users. Adding sample data...");
                }

                //var usersAndMessages = new[] { "haim", "shimon", "david", "adi", "adi23", "adi42", "adi_88", "moris", "avner", "amit_aa", "amit99" };

                var usersAndMessages = GetData();
                await RegisterUsers(usersAndMessages.Keys.ToArray());
                await AddMessages(usersAndMessages);
                _logger.LogInformation("AddSampleDataIfEmptyProject - Done.");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Couldn't add sample data to project. Error.");
            }
        }

        private async Task RegisterUsers(string[] users)
        {
            List<Task> registrationTasks = new List<Task>();
            foreach (var username in users)
            {
                registrationTasks.Add(_usersService.RegisterAsync(username, "Aa##11!!r"));
            }

            await Task.WhenAll(registrationTasks.ToArray());
            _logger.LogInformation($"{users.Length} uesrs were added.");
        }

        private async Task AddMessages(Dictionary<string, string[]> usersAndMessages)
        {
            List<Task> addMessageTasks = new List<Task>();
            foreach (var kvp in usersAndMessages)
            {
                var username = kvp.Key;
                foreach (var message in kvp.Value)
                {
                    var task = _feedsService.AddMessage(username, message);
                    addMessageTasks.Add(task);
                }
            }

            await Task.WhenAll(addMessageTasks);
            _logger.LogInformation($"Messages were added.");
        }

        private Dictionary<string, string[]> GetData()
        {
            var usersAndMessages = new Dictionary<string, string[]> {
                    {
                        "haim", new[] {
                            "Hello!",
                            "What day is it today?",
                            "More is less.",
                            "What do you think about my project?",
                        }
                    },
                    {
                        "david", new[] {
                            "Hello! This is my post!",
                            "I have it!",
                            "What do you think about my project?",
                            "Hello followers!",
                        }
                    },
                    {
                        "adi", new[] {
                            "Hello",
                            "I think I must follow Haim.",
                            "It is so great!",
                        }
                    },
                    {
                        "adi23", new[] {
                            "I love it so much",
                            "Did you watch TV yesterday?",
                            "Oh my god.. I can't believe it.",
                        }
                    },
                    {
                        "adi_online", new[] {
                            "The UI of TwitterPoc is so great! I love it!",
                            "Wow! did you see it??",
                            "Yes, for sure!",
                        }
                    },
                    {
                        "adi1988", new[] {
                            "What should I do now?",
                            "I like to watch cartoons..",
                            "I like Pizza!!",
                        }
                    },
                    {
                        "amit_aa", new[] {
                            "I would like to be a programmer when I grow up.",
                            "I heard the most wonderful joke today.",
                            "Have you ever played the new Civillization 6 PC game??"
                        }
                    },
                    {
                        "amit99", new[] {
                            "Why is my name amit99??",
                            "I'm glad TwitterPoc have so much data built-in. It makes me happy!",
                            "I am going to it Pizza...",
                        }
                    },
                    {
                        "amit", new[] {
                            "I like video games so much!",
                            "I am waiting the holidays...",
                            "What a hot summer.",
                            "It is so simple and fun!",
                        }
                    },
                    {
                      "amore", new[] {
                            "Yes, I'm waiting for it.",
                            "I would like to talk with all my followers",
                            "Hello followers! How are you today?",
                        }
                    },

                };
            return usersAndMessages;
        }
    }
}
