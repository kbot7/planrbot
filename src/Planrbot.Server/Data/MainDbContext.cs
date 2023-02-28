using System.Reflection.Metadata;

using Microsoft.EntityFrameworkCore;

using Planrbot.Models;

namespace Planrbot.Server.Data;

public class MainDbContext : DbContext
{
	public MainDbContext(DbContextOptions<MainDbContext> opt) : base(opt)
	{

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