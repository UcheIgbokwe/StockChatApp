using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Data
{
    public class GroupChatContext : DbContext
        {
            public GroupChatContext(DbContextOptions<GroupChatContext> options)
                : base(options)
            {
            }

            public DbSet<Group> Groups { get; set; }
            public DbSet<Message> Message { get; set; }
            public DbSet<UserGroup> UserGroup { get; set; }
        }
}