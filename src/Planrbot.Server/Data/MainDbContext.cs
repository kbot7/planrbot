namespace Planrbot.Server.Data;

public class MainDbContext : DbContext
{
	public MainDbContext(DbContextOptions<MainDbContext> opt) : base(opt) { }
	public DbSet<PlanrTask> PlanrTasks { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PlanrTask>().HasIndex(e => e.Id).IsUnique();
		modelBuilder.Entity<PlanrTask>().HasKey(e => e.Id);
		modelBuilder.Entity<PlanrTask>()
			.Property(e => e.Id)
			.IsRequired();

		modelBuilder.Entity<PlanrTask>()
			.Property(e => e.Description)
			.HasMaxLength(100)
			.IsRequired();

		modelBuilder.Entity<PlanrTask>()
			.Property(e => e.Date)
			.IsRequired();

		modelBuilder.Entity<PlanrTask>()
			.Property(e => e.IsComplete)
			.IsRequired();
	}
}