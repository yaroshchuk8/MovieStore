using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStore.Models;

namespace MovieStore.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Actor> Actors { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // for identity

			//Seeding a 'Admin' role to AspNetRoles table
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole
				{
					Id = "57bde49e-8d41-45c4-baea-29141e2b2b6c",
					Name = "Customer",
					NormalizedName = "CUSTOMER".ToUpper()
				},
				new IdentityRole 
				{ 
					Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
					Name = "Admin", 
					NormalizedName = "ADMIN".ToUpper() 
				}
				);

			//a hasher to hash the password before seeding the user to the db
			var hasher = new PasswordHasher<IdentityUser>();

			//Seeding the User to AspNetUsers table
			modelBuilder.Entity<IdentityUser>().HasData(
				new IdentityUser
				{
					Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
					UserName = "admin@gmail.com",
					NormalizedUserName = "ADMIN@GMAIL.COM".ToUpper(),
					PasswordHash = hasher.HashPassword(null, "Admin1234_")
				}
			);


			//Seeding the relation between our user and role to AspNetUserRoles table
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
					UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
				}
			);

			//Seeding join table MovieGenre
			modelBuilder.Entity<Movie>()
			  .HasMany(x => x.Genres)
			  .WithMany(y => y.Movies)
			  .UsingEntity(j =>
			  {
				  j.ToTable("MovieGenre");
				  j.HasData(
					  new { GenresId = 1, MoviesId = 1 },
					  new { GenresId = 2, MoviesId = 1 },
					  new { GenresId = 5, MoviesId = 1 },
					  new { GenresId = 4, MoviesId = 2 },
					  new { GenresId = 6, MoviesId = 2 },
					  new { GenresId = 2, MoviesId = 3 },
					  new { GenresId = 3, MoviesId = 3 },
					  new { GenresId = 6, MoviesId = 3 },
					  new { GenresId = 2, MoviesId = 4 },
					  new { GenresId = 5, MoviesId = 4 },
					  new { GenresId = 6, MoviesId = 4 }
					  );
			  });

			//Seeding join table MovieActor
			modelBuilder.Entity<Movie>()
			  .HasMany(x => x.Actors)
			  .WithMany(y => y.Movies)
			  .UsingEntity(j =>
			  {
				  j.ToTable("MovieActor");
				  j.HasData(
					  new { ActorsId = 3, MoviesId = 1 },
					  new { ActorsId = 5, MoviesId = 1 },
					  new { ActorsId = 2, MoviesId = 2 },
					  new { ActorsId = 6, MoviesId = 2 },
					  new { ActorsId = 4, MoviesId = 3 },
					  new { ActorsId = 5, MoviesId = 3 },
					  new { ActorsId = 1, MoviesId = 4 },
					  new { ActorsId = 2, MoviesId = 4 }
					  );
			  });

			//Seeding Genre table
			modelBuilder.Entity<Genre>().HasData(
				new Genre { Id = 1, Name = "Action" },
				new Genre { Id = 2, Name = "Drama" },
				new Genre { Id = 3, Name = "Crime" },
				new Genre { Id = 4, Name = "History" },
				new Genre { Id = 5, Name = "SciFi" },
				new Genre { Id = 6, Name = "Thriller" }
				);

			//Seeding Actor table
			modelBuilder.Entity<Actor>().HasData(
				new Actor { Id = 1, Name = "Leonardo DiCaprio", ImageUrl = @"\images\actor\6753d8f9-22f0-461e-b7b8-ab7f1ea2aa56.jpg" },
				new Actor { Id = 2, Name = "Cillian Murphy", ImageUrl = @"\images\actor\741cbe51-3a8f-4a11-9b28-dc43d5e3b866.jpg" },
				new Actor { Id = 3, Name = "Anne Hathaway", ImageUrl = @"\images\actor\902f1b7f-0655-4f50-8a98-f5e9ba6a7990.jpg" },
				new Actor { Id = 4, Name = "Woody Harrelson", ImageUrl = @"\images\actor\a82fa331-1a40-4c01-8d7a-be71d4e8c2ad.jpg" },
				new Actor { Id = 5, Name = "Matthew McConaughey", ImageUrl = @"\images\actor\b5bb2e2b-d2c4-4dbd-8b4f-e1ac8c9abe29.jpg" },
				new Actor { Id = 6, Name = "Robert Downey Jr.", ImageUrl = @"\images\actor\d572b29f-91c9-4bf9-8d25-2b96f1b52d58.jpg" }
				);

			//Seeding Movie table
			modelBuilder.Entity<Movie>().HasData(
				new Movie
				{
					Id = 1,
					Title = "Interstellar",
					Description = "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",
					Price = 49,
					ImageUrl = @"\images\movie\8ea24e62-0af2-4aa6-8ecb-fd46822b06e3.jpg"
				},
				new Movie
				{
					Id = 2,
					Title = "Oppenheimer",
					Description = "During World War II, Lt. Gen. Leslie Groves Jr. appoints physicist J. Robert Oppenheimer to work on the top-secret Manhattan Project. Oppenheimer and a team of scientists spend years developing and designing the atomic bomb. Their work comes to fruition on July 16, 1945, as they witness the world's first nuclear explosion, forever changing the course of history.",
					Price = 59,
					ImageUrl = @"\images\movie\e9837bd3-d65d-4a1d-b8e4-9c3bed0cf0ed.jpg"
				},
				new Movie
				{
					Id = 3,
					Title = "True Detective",
					Description = "Police officers and detectives around the USA are forced to face dark secrets about themselves and the people around them while investigating homicides.",
					Price = 39,
					ImageUrl = @"\images\movie\4c662a58-4bbe-45cc-96d1-7f14e7870c91.jpg"
				},
				new Movie
				{
					Id = 4,
					Title = "Inception",
					Description = "Cobb steals information from his targets by entering their dreams. He is wanted for his alleged role in his wife's murder and his only chance at redemption is to perform a nearly impossible task.",
					Price = 69,
					ImageUrl = @"\images\movie\de245963-1f4d-44c4-afdb-117660ccf946.jpg"
				}
			);
		}
	}
}
		

