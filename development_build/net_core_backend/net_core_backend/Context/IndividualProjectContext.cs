using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace net_core_backend.Models
{
    public partial class IndividualProjectContext : DbContext
    {
        public IndividualProjectContext()
        {
        }

        public IndividualProjectContext(DbContextOptions<IndividualProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SupportTicket> SupportTicket { get; set; }
        public virtual DbSet<TicketChat> TicketChat { get; set; }
        public virtual DbSet<Transportation> Transportation { get; set; }
        public virtual DbSet<UserKeywords> UserKeywords { get; set; }
        public virtual DbSet<UserTripLocations> UserTripLocations { get; set; }
        public virtual DbSet<UserTrips> UserTrips { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }
        public virtual DbSet<WishListLocations> WishListLocations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SupportTicket)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupportTicket_Users");
            });

            modelBuilder.Entity<TicketChat>(entity =>
            {
                entity.HasKey(e => e.ChatId);

                entity.Property(e => e.ChatId).HasColumnName("chat_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(300);

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketChat)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketChat_SupportTicket");
            });

            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.TransportId);

                entity.Property(e => e.TransportId).HasColumnName("transport_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserKeywords>(entity =>
            {
                entity.HasKey(e => e.KeywordId);

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasColumnName("keyword")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserKeywords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserKeywords_Users");
            });

            modelBuilder.Entity<UserTripLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Lang).HasColumnName("lang");

                entity.Property(e => e.Long).HasColumnName("long");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.TripId).HasColumnName("trip_id");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.UserTripLocations)
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTripLocations_UserTrips");
            });

            modelBuilder.Entity<UserTrips>(entity =>
            {
                entity.HasKey(e => e.TripId);

                entity.Property(e => e.TripId).HasColumnName("trip_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.TransporationId).HasColumnName("transporation_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Transporation)
                    .WithMany(p => p.UserTrips)
                    .HasForeignKey(d => d.TransporationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTrips_Transportation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTrips)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTrips_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Notifications)
                    .IsRequired()
                    .HasColumnName("notifications")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Suggestions)
                    .IsRequired()
                    .HasColumnName("suggestions")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.TransportationId).HasColumnName("transportation_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Transportation)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.TransportationId)
                    .HasConstraintName("FK_WishList_Transportation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishList_Users");
            });

            modelBuilder.Entity<WishListLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Lang).HasColumnName("lang");

                entity.Property(e => e.Long).HasColumnName("long");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

                entity.HasOne(d => d.Wishlist)
                    .WithMany(p => p.WishListLocations)
                    .HasForeignKey(d => d.WishlistId)
                    .HasConstraintName("FK_WishListLocations_WishList");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
