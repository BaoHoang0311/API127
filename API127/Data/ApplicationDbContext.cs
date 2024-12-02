using API127.Models;
using ERP.Infrastructure.Persistence.Configurations.ERP;
using ERP.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace API127.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly AuditableSaveChangesInterceptor _auditableInterceptor;
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableSaveChangesInterceptor auditableInterceptor) : base(options)
        {
            _auditableInterceptor = auditableInterceptor;
        }
        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<API127.Models.VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VillaConfiguration());


            modelBuilder.Entity<Villa>().HasData(
                    new Villa
                    {
                        Id = 1,
                        Name = "Royal Villa",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                        Occupancy = 4,
                        Rate = 200,
                        Sqft = 550,
                        Amenitys = "",

                    },
                    new Villa
                    {
                        Id = 2,
                        Name = "Premium Pool Villa",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                        Occupancy = 4,
                        Rate = 300,
                        Sqft = 550,
                        Amenitys = "",

                    },
                    new Villa
                    {
                        Id = 3,
                        Name = "Luxury Pool Villa",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                        Occupancy = 4,
                        Rate = 400,
                        Sqft = 750,
                        Amenitys = "",

                    },
                    new Villa
                    {
                        Id = 4,
                        Name = "Diamond Villa",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa5.jpg",
                        Occupancy = 4,
                        Rate = 550,
                        Sqft = 900,
                        Amenitys = "",

                    },
                    new Villa
                    {
                        Id = 5,
                        Name = "Diamond Pool Villa",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                        Occupancy = 4,
                        Rate = 600,
                        Sqft = 1100,
                        Amenitys = "",

                    },
                    new Villa
                    {
                        Id = 6,
                        Name = "TEST 1",
                        Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                        Occupancy = 4,
                        Rate = 600,
                        Sqft = 1100,
                        Amenitys = "",

                    },
                    new
                    {
                        Id = 39,
                        Amenity = "",
                        CreatedDate = new DateTime(2024, 3, 22, 0, 34, 11, 955, DateTimeKind.Local).AddTicks(5358),
                        Details = "dsadsad auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                        Name = "39",
                        Occupancy = 4,
                        Rate = 1600.0,
                        Sqft = 1100,
                        UpdatedDate = new DateTime(2024, 3, 22, 0, 34, 11, 955, DateTimeKind.Local).AddTicks(5375)
                    });
            OnModelCreatingPartial(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableInterceptor);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
