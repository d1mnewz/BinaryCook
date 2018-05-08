using System.Threading.Tasks;
using BinaryCook.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace BinaryCook.Infrastructure.Core.Data
{
	public class BinaryCookContextInitializer : IServiceInitializer
	{
		private readonly BinaryCookDbContext _context;

		public BinaryCookContextInitializer(BinaryCookDbContext context)
		{
			_context = context;
		}

		public async Task InitializeAsync()
		{
			//context.Database.EnsureCreated();
			await _context.Database.MigrateAsync();
			//context.Seed();
		}
	}
}