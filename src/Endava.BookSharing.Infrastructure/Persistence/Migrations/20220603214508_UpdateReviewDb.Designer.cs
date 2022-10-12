﻿// <auto-generated />
using System;
using Endava.BookSharing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Endava.BookSharing.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220603214508_UpdateReviewDb")]
    partial class UpdateReviewDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookDbGenreDb", b =>
                {
                    b.Property<string>("BooksId")
                        .HasColumnType("text");

                    b.Property<string>("GenresId")
                        .HasColumnType("text");

                    b.HasKey("BooksId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("BookDbGenreDb");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.RoleDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.AuthorDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AddedById")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(61)
                        .HasColumnType("character varying(61)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.BookDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CoverId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("boolean");

                    b.Property<string>("LanguageId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CoverId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.FileDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Raw")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.GenreDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.LanguageDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.ReviewDb", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BookId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("PostedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PostedById")
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("PostedById");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookDbGenreDb", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.BookDb", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.GenreDb", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserRole", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.RoleDb", "RoleDb")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", "UserDb")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleDb");

                    b.Navigation("UserDb");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.AuthorDb", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.Navigation("AddedBy");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.BookDb", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.AuthorDb", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.FileDb", "Cover")
                        .WithMany()
                        .HasForeignKey("CoverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.LanguageDb", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Cover");

                    b.Navigation("Language");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.ReviewDb", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Models.BookDb", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Reviews_Book");

                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", "PostedBy")
                        .WithMany()
                        .HasForeignKey("PostedById");

                    b.Navigation("Book");

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.RoleDb", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.RoleDb", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Identity.Models.UserDb", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Endava.BookSharing.Infrastructure.Persistence.Models.BookDb", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
