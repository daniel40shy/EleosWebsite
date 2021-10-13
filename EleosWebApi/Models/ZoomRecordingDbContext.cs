using Microsoft.EntityFrameworkCore;

namespace EleosWebApi.Models
{
    public class ZoomRecordingDbContext : DbContext
    {
        public ZoomRecordingDbContext(DbContextOptions<ZoomRecordingDbContext> options) : base(options)
        {
        }

        public DbSet<ZoomRecording> ZoomRecordings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZoomRecording>().ToTable("ZoomRecordings");
        }
    }
}
