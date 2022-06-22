using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<CartConfig> CartConfig { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Cleaning> Cleaning { get; set; }
        public virtual DbSet<DigitalMenu> DigitalMenu { get; set; }
        public virtual DbSet<Directory> Directory { get; set; }
        public virtual DbSet<DirectoryDetail> DirectoryDetail { get; set; }
        public virtual DbSet<DirectoryHead> DirectoryHead { get; set; }
        public virtual DbSet<HomeMenu> HomeMenu { get; set; }
        public virtual DbSet<HotelAssets> HotelAssets { get; set; }
        public virtual DbSet<Hotels> Hotels { get; set; }
        public virtual DbSet<InfoPage> InfoPage { get; set; }
        public virtual DbSet<InfoPageDetail> InfoPageDetail { get; set; }
        public virtual DbSet<IntakeReason> IntakeReason { get; set; }
        public virtual DbSet<LateCheckout> LateCheckout { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderHead> OrderHead { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<PaymType> PaymType { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Promo> Promo { get; set; }
        public virtual DbSet<PromoType> PromoType { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<Reason> Reason { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<StatusNames> StatusNames { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=Conn");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CartConfig>(entity =>
            {
                entity.HasKey(e => new { e.IdProvider, e.HotelCode });

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DaysOfWeek)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HourEnd)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.HourStart)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdProviderNavigation)
                    .WithMany(p => p.CartConfig)
                    .HasForeignKey(d => d.IdProvider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartConfig_Provider");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NameEng)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Cleaning>(entity =>
            {
                entity.HasKey(e => e.HotelCode)
                    .HasName("PK__Cleaning__175CAD5911448E8D");

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<DigitalMenu>(entity =>
            {
                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Directory>(entity =>
            {
                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectoryDetail>(entity =>
            {
                entity.Property(e => e.BannerUrl).IsUnicode(false);

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IconUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectoryHead>(entity =>
            {
                entity.HasKey(e => e.HotelCode)
                    .HasName("PK__Director__175CAD590D7E34E0");

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BannerUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Introduction).IsUnicode(false);
            });

            modelBuilder.Entity<HomeMenu>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.LinkUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotelAssets>(entity =>
            {
                entity.HasKey(e => e.HotelCode)
                    .HasName("PK__HotelAss__175CAD59B7A7A1B2");

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginBackground)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LogoSmall)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Hotels>(entity =>
            {
                entity.HasKey(e => e.Url)
                    .HasName("PK__Hotels__3214EC078BC9FFA1");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HoursToAgreeBeforeCheckout)
                    .HasColumnName("hoursToAgreeBeforeCheckout")
                    .HasDefaultValueSql("((6))");

                entity.Property(e => e.Image)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ImageRestaurants)
                    .HasColumnName("imageRestaurants")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MinutesBfast).HasColumnName("MinutesBFast");

                entity.Property(e => e.Rmc)
                    .HasColumnName("RMC")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShowRestaurants).HasColumnName("showRestaurants");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InfoPage>(entity =>
            {
                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InfoPageDetail>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.LinkUrl).IsUnicode(false);

                entity.Property(e => e.MapUrl).IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdInfoPageNavigation)
                    .WithMany(p => p.InfoPageDetail)
                    .HasForeignKey(d => d.IdInfoPage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InfoPage_InfoPageDetail");
            });

            modelBuilder.Entity<IntakeReason>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LateCheckout>(entity =>
            {
                entity.Property(e => e.ConfirmText)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Modifiers).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.IdOrderHeadNavigation)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.IdOrderHead)
                    .HasConstraintName("FK_OrderDetail_OrderHead");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK__OrderDeta__IdPro__4AB81AF0");
            });

            modelBuilder.Entity<OrderHead>(entity =>
            {
                entity.Property(e => e.BreakFastDate).HasColumnType("date");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubTotal).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.IdIntakeReasonNavigation)
                    .WithMany(p => p.OrderHead)
                    .HasForeignKey(d => d.IdIntakeReason)
                    .HasConstraintName("FK_OrderHead_IntakeReason");

                entity.HasOne(d => d.IdPaymTypeNavigation)
                    .WithMany(p => p.OrderHead)
                    .HasForeignKey(d => d.IdPaymType)
                    .HasConstraintName("FK_OrderHead_PaymType");

                entity.HasOne(d => d.IdPromoNavigation)
                    .WithMany(p => p.OrderHead)
                    .HasForeignKey(d => d.IdPromo)
                    .HasConstraintName("FK_OrderHead_Promo");

                entity.HasOne(d => d.IdProviderNavigation)
                    .WithMany(p => p.OrderHead)
                    .HasForeignKey(d => d.IdProvider)
                    .HasConstraintName("FK_OrderHead_Provider");

                entity.HasOne(d => d.IdReservationNavigation)
                    .WithMany(p => p.OrderHead)
                    .HasForeignKey(d => d.IdReservation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHead_Reservation");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAppUserNavigation)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.IdAppUser)
                    .HasConstraintName("FK_OrderStatus_AppUser");

                entity.HasOne(d => d.IdOrderHeadNavigation)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.IdOrderHead)
                    .HasConstraintName("FK_OrderStatus_OrderHead");

                entity.HasOne(d => d.IdReasonNavigation)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.IdReason)
                    .HasConstraintName("FK_OrderStatus_Reason");
            });

            modelBuilder.Entity<PaymType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdProductTypeNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdProductType)
                    .HasConstraintName("FK_Product_ProductType");

                entity.HasOne(d => d.IdProviderNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdProvider)
                    .HasConstraintName("FK_Product_Provider");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Modifiers).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Updated).HasColumnType("datetime");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.ProductType)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_ProductType_Category");
            });

            modelBuilder.Entity<Promo>(entity =>
            {
                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateStart).HasColumnType("date");

                entity.HasOne(d => d.IdPromoTypeNavigation)
                    .WithMany(p => p.Promo)
                    .HasForeignKey(d => d.IdPromoType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promo_PromoType");
            });

            modelBuilder.Entity<PromoType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CountryDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Document)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LoyaltyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MarketSegmentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.RoomCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HourEnd)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.HourStart)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Slider>(entity =>
            {
                entity.Property(e => e.HotelCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasColumnName("ImageURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LinkUrl)
                    .HasColumnName("LinkURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusNames>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.HasOne(d => d.IdReservationNavigation)
                    .WithMany(p => p.UserLog)
                    .HasForeignKey(d => d.IdReservation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLog_Reservation");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
