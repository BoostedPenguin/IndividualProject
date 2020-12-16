using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using net_core_backend.Models;

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

        public virtual DbSet<KeywordAddress> KeywordAddress { get; set; }
        public virtual DbSet<KeywordType> KeywordType { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<SupportTicket> SupportTicket { get; set; }
        public virtual DbSet<TicketChat> TicketChat { get; set; }
        public virtual DbSet<UserKeywords> UserKeywords { get; set; }
        public virtual DbSet<UserTrips> UserTrips { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KeywordAddress>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.CityAlt)
                    .HasColumnName("city_alt")
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.CountryCode)
                    .HasColumnName("country_code")
                    .HasMaxLength(20);

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

            });

            modelBuilder.Entity<KeywordType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.KeywordType)
                    .HasForeignKey(d => d.KeywordId)
                    .HasConstraintName("FK_KeywordType_UserKeywords");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasIndex(e => e.TripId);

                entity.HasIndex(e => e.WishlistId);

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("FK_Locations_UserTrips");

                entity.HasOne(d => d.Wishlist)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.WishlistId)
                    .HasConstraintName("FK_Locations_WishList");
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SupportTicket)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SupportTicket_Users");
            });

            modelBuilder.Entity<TicketChat>(entity =>
            {
                entity.HasIndex(e => e.TicketId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(300);

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketChat)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_TicketChat_SupportTicket");
            });

            modelBuilder.Entity<UserKeywords>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasColumnName("keyword")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(a => a.KeywordAddress)
                    .WithOne(b => b.Keyword)
                    .HasForeignKey<KeywordAddress>(e => e.KeywordId)
                    .HasConstraintName("FK_KeywordAddress_UserKeywords");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserKeywords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserKeywords_Users");
            });

            modelBuilder.Entity<UserTrips>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.Transportation)
                    .IsRequired()
                    .HasConversion(x => x.ToString(), x => (Transportation)Enum.Parse(typeof(Transportation), x))
                    .HasColumnName("transportation")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTrips)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserTrips_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Auth)
                    .HasName("Auth_Unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Auth)
                    .IsRequired()
                    .HasColumnName("auth")
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

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasConversion(x => x.ToString(), x => (Role)Enum.Parse(typeof(Role), x))
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('User')");

                entity.Property(e => e.Suggestions)
                    .IsRequired()
                    .HasColumnName("suggestions")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Transportation)
                    .HasConversion(x => x.ToString(), x => (Transportation)Enum.Parse(typeof(Transportation), x))
                    .HasColumnName("transportation")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_WishList_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
