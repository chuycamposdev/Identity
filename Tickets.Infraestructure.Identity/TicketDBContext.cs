using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entities;
using Tickets.Infraestructure.Identity.Extensions;
using Tickets.Infraestructure.Identity.Models;

namespace Tickets.Infraestructure.Identity
{
    public class TicketDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefeshToken { get; set; }
        public TicketDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}
