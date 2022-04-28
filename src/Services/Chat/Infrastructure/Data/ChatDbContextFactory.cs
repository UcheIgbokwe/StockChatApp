using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data
{
    public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
    {
        public ChatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
            //optionsBuilder.UseSqlServer("Server=chatdb;Database=ChatDb;User Id=sa;Password=Ebubechi89;");
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1433;Initial Catalog=ChatDb;User Id=sa;Password=Ebubechi89;");
            return new ChatDbContext(optionsBuilder.Options);
        }
    }
}