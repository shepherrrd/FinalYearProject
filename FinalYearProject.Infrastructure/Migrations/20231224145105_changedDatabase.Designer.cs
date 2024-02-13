﻿// <auto-generated />
using System;
using System.Collections.Generic;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    [DbContext(typeof(FinalYearDBContext))]
    [Migration("20231224145105_changedDatabase")]
    partial class changedDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.ApplicationRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTimeOffset>("TimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CCode")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("IsoCode")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("TimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Country");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CCode = "NGN",
                            Code = "+234",
                            Currency = "₦",
                            Icon = "countryimages/nigeria.png",
                            IsoCode = "NG",
                            Name = "Nigeria",
                            TimeCreated = new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 833, DateTimeKind.Unspecified).AddTicks(9570), new TimeSpan(0, 0, 0, 0, 0)),
                            TimeUpdated = new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 833, DateTimeKind.Unspecified).AddTicks(9573), new TimeSpan(0, 0, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.DataAggregatorUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<int?>("CountryID")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<long?>("HospitalInfo")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastLoginDevice")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

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

                    b.Property<long?>("ResearchCenterInfo")
                        .HasColumnType("bigint");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("TimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AccessFailedCount = 0,
                            AccountStatus = 1,
                            ConcurrencyStamp = "7df0d7d8-f5f7-4b78-bcc0-e6e8c18faadc",
                            Email = "technology@dataaggregator.com",
                            EmailConfirmed = true,
                            FirstName = "FInalYear",
                            LastLogin = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LastName = "Administrator",
                            LockoutEnabled = false,
                            NormalizedEmail = "TECHNOLOGY@DATAAGGREGATOR.COM",
                            NormalizedUserName = "TECHNOLOGY@DATAAGGREGATOR.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEMzdtY7g+NLh9CNuU+OLwD1B8oWmlH41uj+IAhrEPY0Yczt2X6q6eiymkxj8Idn1MQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "B5E07B2C21E026478FD07BD0EABAF37E",
                            TimeCreated = new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 831, DateTimeKind.Unspecified).AddTicks(7492), new TimeSpan(0, 0, 0, 0, 0)),
                            TimeUpdated = new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 831, DateTimeKind.Unspecified).AddTicks(7496), new TimeSpan(0, 0, 0, 0, 0)),
                            TwoFactorEnabled = false,
                            UserName = "TECHNOLOGY@DATAAGGREGATOR.COM",
                            UserType = 1
                        });
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.HospitalInfo", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<int?>("CacDocumentID")
                        .HasColumnType("integer");

                    b.Property<string>("HospitalName")
                        .HasColumnType("text");

                    b.Property<string>("HospitalWebsite")
                        .HasColumnType("text");

                    b.Property<int?>("NafdacDocumentID")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("HospitalInfos");
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.OtpVerification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ConfirmedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Purpose")
                        .HasColumnType("integer");

                    b.Property<string>("Recipient")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RecipientType")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("TimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("OtpVerifications");
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<int>>("Documents")
                        .HasColumnType("integer[]");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("RegistrationRequests");
                });

            modelBuilder.Entity("FinalYearProject.Infrastructure.Data.Entities.ResearchCenterInfo", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<int?>("DegreeID")
                        .HasColumnType("integer");

                    b.Property<int?>("IRBApprovalID")
                        .HasColumnType("integer");

                    b.Property<string>("Institution")
                        .HasColumnType("text");

                    b.Property<int?>("PassportID")
                        .HasColumnType("integer");

                    b.Property<int?>("ResearchProposalID")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("ResearchCenterInfo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "ActivityLogs",
                            ClaimValue = "ActivityLogs",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "Dashboard",
                            ClaimValue = "Dashboard",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "Users",
                            ClaimValue = "Users",
                            UserId = 1L
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.DataAggregatorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.DataAggregatorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.DataAggregatorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("FinalYearProject.Infrastructure.Data.Entities.DataAggregatorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}