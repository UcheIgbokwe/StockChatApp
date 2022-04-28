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
    public static class EventDbContextSeed
    {
        public static async Task SeedAsync(ChatDbContext chatDbContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryforAvailability = retry!.Value;
            var log = loggerFactory.CreateLogger<ChatDbContext>();

            try
            {
                await chatDbContext.Database.MigrateAsync();
                //Seed data for Users
                if (chatDbContext.Users.Any()) return;
                var users = GetPreConfiguredUsers();

                foreach (var user in users)
                {
                    user.UserName = user?.UserName?.ToLower();
                    user!.PasswordHash = "Pa$$w0rd";
                    user.Role = Role.User;
                    chatDbContext.Users.Add(user);
                    chatDbContext.SaveChanges();
                }

                var adminUser = new User
                {
                    Email = "admin@test.com",
                    UserName = "admin",
                    PhoneNumber = "08063788006"
                };

                adminUser.PasswordHash = "Pa$$w0rd";
                adminUser.Role = Role.Admin;
                chatDbContext.Users.Add(adminUser);
                chatDbContext.SaveChanges();
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
        private static IEnumerable<User> GetPreConfiguredUsers()
        {
            return new List<User>()
            {
                new User() { UserName = "ucheIgbokwe", Email = "uche@ymail.com", PhoneNumber = "08063788008"},
                new User() { UserName = "henryIgbokwe", Email = "henry@ymail.com", PhoneNumber = "08063788009"}
            };
        }
    }
}