using Framework.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Framework.Core.DbContextCore.CodeFirst
{
    public class PostgreSQLDbContext : BaseDbContext
    {
        public PostgreSQLDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
