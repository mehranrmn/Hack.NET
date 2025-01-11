using Microsoft.EntityFrameworkCore;


namespace Files.Models {
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options) 
        {
        }

        public DbSet<File> FileItems { get; set; }
        public DbSet<Metadata> MetadataEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasOne(f => f.Metadata)
                .WithOne(m => m.File)
                .HasForeignKey<Metadata>(m => m.FileId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Metadata>()
                .HasIndex(m => m.FileId)
                .IsUnique();
        }
    }
}

