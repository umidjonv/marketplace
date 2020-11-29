using Catalog.Data.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Data.Entities;


namespace Catalog.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        private readonly IHttpContextAccessor _httpContext;

        private void ApplyAuditValues()
        {
            var entries = ChangeTracker.Entries()
               .Where(e => e.Entity is AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            // user
            var userName = _httpContext?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = Environment.UserName ?? "(local)";
            }

            // ip
            var userIp = _httpContext?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            if (string.IsNullOrWhiteSpace(userIp))
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                userIp = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "::1";
            }

            foreach (var entityEntry in entries)
            {
                ((AuditEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;
                ((AuditEntity)entityEntry.Entity).ModifiedIp = userIp;
                ((AuditEntity)entityEntry.Entity).ModifiedBy = userName;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((AuditEntity)entityEntry.Entity).CreatedIp = userIp;
                    ((AuditEntity)entityEntry.Entity).CreatedBy = userName;
                }
            }

        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContext)
           : base(options)
        {
            _httpContext = httpContext;
        }

        //public DbSet<User> Users { get; set; }


        public DbSet<Product> Products { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditValues();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditValues();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            

            //modelBuilder.Entity<User>(schema =>
            //{
            //    schema.HasKey(a => a.Id);

            //    schema.HasIndex(a => a.OwnerId).IsUnique();
            //});
             
            

            base.OnModelCreating(modelBuilder);
        }

    }
}
