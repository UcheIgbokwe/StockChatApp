using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public static class ChatDbContextSeed
    {
        public static async Task SeedAsync(ChatDbContext chatDbContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryforAvailability = retry!.Value;
            var log = loggerFactory.CreateLogger<ChatDbContext>();

            try
            {
                await chatDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                if (retryforAvailability < 6)
                {
                    retryforAvailability++;
                    log.LogError("Error occured: {Ex.Message}", ex.Message);
                    await SeedAsync(chatDbContext, loggerFactory, retryforAvailability);
                }
            }
        }
    }
}