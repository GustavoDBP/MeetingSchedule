using MeetingSchedule.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

namespace MeetingSchedule.DBContext
{
    public class MeetingScheduleContext : DbContext
    {
        public MeetingScheduleContext(DbContextOptions<MeetingScheduleContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MeetingSchedule");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}