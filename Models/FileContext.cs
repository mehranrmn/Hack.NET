using Microsoft.EntityFrameworkCore;

namespace Files.Models;

public class FileContext : DbContext
{
    public FileContext(DbContextOptions<FileContext> options) : base(options) 
    {
    }

    public DbSet<File> FileItems { get; set; }
}