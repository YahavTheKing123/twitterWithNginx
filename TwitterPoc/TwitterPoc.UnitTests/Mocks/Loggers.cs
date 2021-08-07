using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Authorization;
using TwitterPoc.Logic.Services;

namespace TwitterPoc.UnitTests.Mocks
{
    public partial class LoggerMock : ILogger<TokenService> { }
    public partial class LoggerMock : ILogger<UsersService> { }
    public partial class LoggerMock : ILogger<FeedsService> { }
    public partial class LoggerMock : ILogger
    {
        class DisposableClass : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new DisposableClass(); 
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter){}
    }
}
