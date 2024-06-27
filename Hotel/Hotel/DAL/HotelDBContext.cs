using Hotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.DAL
{
    public class HotelDBContext : IdentityDbContext
    {
        public HotelDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutCompany> AboutCompanies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ReservationService> ReservationServices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<UserReservation> UserReservations { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            builder.Entity<UserReservation>()
                .HasKey(x => new { x.ReservationId, x.AppUserId });

			builder.Entity<UserReservation>()
				.HasOne(x => x.AppUser)
				.WithMany(xt => xt.UserReservations)
				.HasForeignKey(x => x.AppUserId)
				.OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<UserReservation>()
				.HasOne(x => x.Reservation)
				.WithMany(xt => xt.UserReservations)
				.HasForeignKey(x => x.ReservationId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
