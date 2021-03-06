﻿// <auto-generated />
using Cinematicks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Cinematicks.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cinematicks.Models.Cinema", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.Property<int>("LocationID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("PhotoID");

                    b.Property<float>("Price");

                    b.HasKey("ID");

                    b.HasIndex("LocationID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("PhotoID");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("Cinematicks.Models.Client", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AvatarID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AvatarID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Cinematicks.Models.Genre", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<bool>("InMenu");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Cinematicks.Models.Hall", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CinemaID");

                    b.Property<int>("Cols");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("Rows");

                    b.HasKey("ID");

                    b.HasIndex("CinemaID", "Name")
                        .IsUnique();

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("Cinematicks.Models.Image", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("Filename");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<DateTime>("UploadedTime")
                        .HasColumnType("smalldatetime");

                    b.HasKey("ID");

                    b.HasIndex("Filename")
                        .IsUnique()
                        .HasFilter("[Filename] IS NOT NULL");

                    b.HasIndex("Category", "Name")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Cinematicks.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Directions")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("MapID");

                    b.HasKey("ID");

                    b.HasIndex("MapID");

                    b.HasIndex("City", "Address")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Cinematicks.Models.MovGen", b =>
                {
                    b.Property<int>("MovieID");

                    b.Property<int>("GenreID");

                    b.HasKey("MovieID", "GenreID");

                    b.HasIndex("GenreID");

                    b.ToTable("MovGens");
                });

            modelBuilder.Entity("Cinematicks.Models.Movie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Actors")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<bool>("IsDub");

                    b.Property<string>("Plot")
                        .IsRequired()
                        .HasMaxLength(1500);

                    b.Property<int>("PosterID");

                    b.Property<int>("RateID");

                    b.Property<DateTime>("Release")
                        .HasColumnType("date");

                    b.Property<int>("Time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Trailer")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.HasKey("ID");

                    b.HasIndex("PosterID");

                    b.HasIndex("RateID");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.HasIndex("Trailer")
                        .IsUnique();

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Cinematicks.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientID")
                        .IsRequired();

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("smalldatetime");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Cinematicks.Models.Payment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(80);

                    b.Property<string>("CCCVV")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CCID")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<int>("CCMonth");

                    b.Property<string>("CCNum")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<int>("CCYear");

                    b.Property<string>("City")
                        .HasMaxLength(30);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("OrderID");

                    b.Property<bool>("SendInvoice");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(7);

                    b.HasKey("ID");

                    b.HasIndex("OrderID")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Cinematicks.Models.PhotoGal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CinemaID");

                    b.Property<string>("Description")
                        .HasMaxLength(150);

                    b.Property<bool>("IsActive");

                    b.Property<int>("PhotoID");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("ID");

                    b.HasIndex("PhotoID");

                    b.HasIndex("CinemaID", "Title")
                        .IsUnique();

                    b.ToTable("Gallery");
                });

            modelBuilder.Entity("Cinematicks.Models.Promo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BannerID");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(800);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("date");

                    b.HasKey("ID");

                    b.HasIndex("BannerID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Promos");
                });

            modelBuilder.Entity("Cinematicks.Models.Rate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("ImageID");

                    b.Property<int>("MinAge");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ImageID");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("Cinematicks.Models.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientID")
                        .IsRequired();

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("MovieID");

                    b.Property<DateTime>("PostTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("Rating");

                    b.HasKey("ID");

                    b.HasIndex("MovieID");

                    b.HasIndex("ClientID", "MovieID")
                        .IsUnique();

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Cinematicks.Models.Show", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HallID");

                    b.Property<int>("MovieID");

                    b.Property<DateTime>("ShowDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("ShowTime")
                        .HasColumnType("time");

                    b.HasKey("ID");

                    b.HasIndex("MovieID");

                    b.HasIndex("HallID", "ShowDate", "ShowTime")
                        .IsUnique();

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("Cinematicks.Models.Ticket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientID")
                        .IsRequired();

                    b.Property<int>("Col");

                    b.Property<int>("OrderID");

                    b.Property<int>("Row");

                    b.Property<int>("ShowID");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ShowID", "Row", "Col")
                        .IsUnique();

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Cinematicks.Models.Cinema", b =>
                {
                    b.HasOne("Cinematicks.Models.Location", "Location")
                        .WithMany("Cinemas")
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Image", "Photo")
                        .WithMany("Cinemas")
                        .HasForeignKey("PhotoID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Client", b =>
                {
                    b.HasOne("Cinematicks.Models.Image", "Avatar")
                        .WithMany("Avatars")
                        .HasForeignKey("AvatarID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Hall", b =>
                {
                    b.HasOne("Cinematicks.Models.Cinema", "Cinema")
                        .WithMany("Halls")
                        .HasForeignKey("CinemaID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Location", b =>
                {
                    b.HasOne("Cinematicks.Models.Image", "Map")
                        .WithMany("Locations")
                        .HasForeignKey("MapID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.MovGen", b =>
                {
                    b.HasOne("Cinematicks.Models.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Movie", "Movie")
                        .WithMany("Genres")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Movie", b =>
                {
                    b.HasOne("Cinematicks.Models.Image", "Poster")
                        .WithMany("Movies")
                        .HasForeignKey("PosterID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Rate", "Rate")
                        .WithMany("Movies")
                        .HasForeignKey("RateID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Order", b =>
                {
                    b.HasOne("Cinematicks.Models.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Payment", b =>
                {
                    b.HasOne("Cinematicks.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Cinematicks.Models.Payment", "OrderID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cinematicks.Models.PhotoGal", b =>
                {
                    b.HasOne("Cinematicks.Models.Cinema", "Cinema")
                        .WithMany("Gallery")
                        .HasForeignKey("CinemaID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Image", "Photo")
                        .WithMany("Gallery")
                        .HasForeignKey("PhotoID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Promo", b =>
                {
                    b.HasOne("Cinematicks.Models.Image", "Banner")
                        .WithMany("Banners")
                        .HasForeignKey("BannerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Rate", b =>
                {
                    b.HasOne("Cinematicks.Models.Image", "Image")
                        .WithMany("Rates")
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Review", b =>
                {
                    b.HasOne("Cinematicks.Models.Client", "Client")
                        .WithMany("Reviews")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Show", b =>
                {
                    b.HasOne("Cinematicks.Models.Hall", "Hall")
                        .WithMany("Shows")
                        .HasForeignKey("HallID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Movie", "Movie")
                        .WithMany("Shows")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Cinematicks.Models.Ticket", b =>
                {
                    b.HasOne("Cinematicks.Models.Client", "Client")
                        .WithMany("Tickets")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Cinematicks.Models.Order", "Order")
                        .WithMany("Tickets")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cinematicks.Models.Show", "Show")
                        .WithMany("Tickets")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Cinematicks.Models.Client")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Cinematicks.Models.Client")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cinematicks.Models.Client")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Cinematicks.Models.Client")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
