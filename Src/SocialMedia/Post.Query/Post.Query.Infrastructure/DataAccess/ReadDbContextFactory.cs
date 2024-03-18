using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
	public class ReadDbContextFactory
	{
		private readonly Action<DbContextOptionsBuilder> _configDbContext;

		public ReadDbContextFactory(Action<DbContextOptionsBuilder> configDbContext)
		{
			_configDbContext = configDbContext;
		}

		public ReadDbContext CreateDbContext()
		{
			DbContextOptionsBuilder<ReadDbContext> options = new();
			_configDbContext(options);

			return new ReadDbContext(options.Options);
		}
	}
}
