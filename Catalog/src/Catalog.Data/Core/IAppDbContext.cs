using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Data.Entities;


namespace Catalog.Data.Core
{
    public interface IAppDbContext
    {
        public DbSet<Product> Products { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}