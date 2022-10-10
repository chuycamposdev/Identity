using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Infraestructure.Identity
{
    public class TicketContextFactory : IDesignTimeDbContextFactory<TicketDBContext>
    {
        public TicketDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<TicketDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Identity"));

            return new TicketDBContext(optionsBuilder.Options);
        }
    }
}
