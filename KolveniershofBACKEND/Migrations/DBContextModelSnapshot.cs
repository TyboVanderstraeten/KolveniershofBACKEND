﻿// <auto-generated />
using System;
using KolveniershofBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivityType")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Pictogram")
                        .IsRequired();

                    b.HasKey("ActivityId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Attendance", b =>
                {
                    b.Property<int>("DayId");

                    b.Property<int>("ActivityId");

                    b.Property<int>("UserId");

                    b.Property<string>("TimeOfDay");

                    b.Property<string>("Comment");

                    b.HasKey("DayId", "ActivityId", "UserId", "TimeOfDay");

                    b.HasIndex("UserId");

                    b.HasIndex("DayId", "ActivityId", "TimeOfDay");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.BusDriver", b =>
                {
                    b.Property<int>("DayId");

                    b.Property<int>("DriverId");

                    b.Property<string>("TimeOfDay");

                    b.Property<string>("BusColor")
                        .IsRequired();

                    b.HasKey("DayId", "DriverId", "TimeOfDay");

                    b.HasIndex("DriverId");

                    b.ToTable("BusDriver");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Day", b =>
                {
                    b.Property<int>("DayId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DayNr");

                    b.Property<string>("DayType")
                        .IsRequired();

                    b.Property<string>("TemplateName")
                        .IsRequired();

                    b.Property<int>("WeekNr");

                    b.HasKey("DayId");

                    b.ToTable("Day");

                    b.HasDiscriminator<string>("DayType").HasValue("template_day");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.DayActivity", b =>
                {
                    b.Property<int>("DayId");

                    b.Property<int>("ActivityId");

                    b.Property<string>("TimeOfDay");

                    b.HasKey("DayId", "ActivityId", "TimeOfDay");

                    b.HasIndex("ActivityId");

                    b.ToTable("DayActivity");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("DriverId");

                    b.ToTable("Driver");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Helper", b =>
                {
                    b.Property<int>("DayId");

                    b.Property<int>("UserId");

                    b.HasKey("DayId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Helper");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("DayId");

                    b.Property<string>("NoteType")
                        .IsRequired();

                    b.HasKey("NoteId");

                    b.HasIndex("DayId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int?>("Group");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("ProfilePicture");

                    b.Property<string>("UserType")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.WeekendDay", b =>
                {
                    b.Property<int>("WeekendDayId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<int>("UserId");

                    b.HasKey("WeekendDayId");

                    b.HasIndex("UserId");

                    b.ToTable("WeekendDay");
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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

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

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.CustomDay", b =>
                {
                    b.HasBaseType("KolveniershofBACKEND.Models.Domain.Day");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Dessert")
                        .IsRequired();

                    b.Property<string>("MainDish")
                        .IsRequired();

                    b.Property<string>("PreDish")
                        .IsRequired();

                    b.ToTable("CustomDay");

                    b.HasDiscriminator().HasValue("custom_day");
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Attendance", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KolveniershofBACKEND.Models.Domain.DayActivity", "DayActivity")
                        .WithMany("Attendances")
                        .HasForeignKey("DayId", "ActivityId", "TimeOfDay")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.BusDriver", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.Day", "Day")
                        .WithMany("BusDrivers")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KolveniershofBACKEND.Models.Domain.Driver", "Driver")
                        .WithMany("DaysToDrive")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.DayActivity", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KolveniershofBACKEND.Models.Domain.Day", "Day")
                        .WithMany("DayActivities")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Helper", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.Day", "Day")
                        .WithMany("Helpers")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KolveniershofBACKEND.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.Note", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.CustomDay")
                        .WithMany("Notes")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KolveniershofBACKEND.Models.Domain.WeekendDay", b =>
                {
                    b.HasOne("KolveniershofBACKEND.Models.Domain.User")
                        .WithMany("WeekendDays")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
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

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
