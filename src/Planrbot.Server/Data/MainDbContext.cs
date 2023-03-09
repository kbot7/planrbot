namespace Planrbot.Server.Data;

public class MainDbContext : DbContext
{
	private readonly IWebHostEnvironment _env;
	private readonly IConfiguration _config;
	public MainDbContext(DbContextOptions<MainDbContext> opt, IWebHostEnvironment env, IConfiguration config) : base(opt)
	{
		_env = env;
		_config = config;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (_env.IsDevelopment()) 
		{
			optionsBuilder.UseInMemoryDatabase("planrbot");
		} else
		{
			optionsBuilder.UseSqlServer(_config.GetConnectionString("Main"));
		}

		base.OnConfiguring(optionsBuilder);	
	}

	public DbSet<ToDoItem> ToDoItems { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ToDoItem>().HasIndex(e => e.Id).IsUnique();
		modelBuilder.Entity<ToDoItem>().HasKey(e => e.Id);
		modelBuilder.Entity<ToDoItem>()
			.Property(e => e.Id)
			.IsRequired();

		modelBuilder.Entity<ToDoItem>()
			.Property(e => e.Description)
			.HasMaxLength(100)
			.IsRequired();

		modelBuilder.Entity<ToDoItem>()
			.Property(e => e.Date)
			.IsRequired();

		modelBuilder.Entity<ToDoItem>()
			.Property(e => e.IsComplete)
			.IsRequired();
	}
}