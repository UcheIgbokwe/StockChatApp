using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.CronJob
{
    public interface IMyScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }

    public class MyScopedService : IMyScopedService
    {
        private readonly ILogger<MyScopedService> _logger;
        private readonly ChatDbContext _chatDbContext;
        private readonly ILoggerFactory _loggerFactory;

        public MyScopedService(ChatDbContext chatDbContext, ILogger<MyScopedService> logger, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _chatDbContext = chatDbContext;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                await ChatDbContextSeed.SeedAsync(_chatDbContext, _loggerFactory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured seeding data");
            }
        }
    }
}