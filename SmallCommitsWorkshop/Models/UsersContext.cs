using Microsoft.EntityFrameworkCore;

namespace SmallCommitsWorkshop.Models {
	public class UsersContext : DbContext {
		public UsersContext( DbContextOptions<UsersContext> options )
			: base( options ) { }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating( ModelBuilder modelBuilder ) {
			modelBuilder.Entity<User>()
				.HasIndex( u => u.Id )
				.IsUnique();
		}
	}
}
