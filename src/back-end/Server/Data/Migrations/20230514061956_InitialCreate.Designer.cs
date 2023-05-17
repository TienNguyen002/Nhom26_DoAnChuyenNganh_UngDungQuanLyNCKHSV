﻿// <auto-generated />
using System;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(WebDbContext))]
    [Migration("20230514061956_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TypeAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LecturerId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TypeAccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Core.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Core.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Core.Entities.Lecturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DoB")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Qualification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("Core.Entities.Process", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Processes");
                });

            modelBuilder.Entity("Core.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Class")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DoB")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("GroupId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Core.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutlineUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResultUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("StudentNumbers")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("GroupId");

                    b.HasIndex("LecturerId");

                    b.HasIndex("ProcessId");

                    b.HasIndex("StatusId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Core.Entities.TypeAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TypeAccounts");
                });

            modelBuilder.Entity("Core.Entities.Account", b =>
                {
                    b.HasOne("Core.Entities.Lecturer", "Lecturer")
                        .WithMany("Accounts")
                        .HasForeignKey("LecturerId");

                    b.HasOne("Core.Entities.Student", "Student")
                        .WithMany("Accounts")
                        .HasForeignKey("StudentId");

                    b.HasOne("Core.Entities.TypeAccount", "TypeAccount")
                        .WithMany("Accounts")
                        .HasForeignKey("TypeAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecturer");

                    b.Navigation("Student");

                    b.Navigation("TypeAccount");
                });

            modelBuilder.Entity("Core.Entities.Group", b =>
                {
                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Groups")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Core.Entities.Lecturer", b =>
                {
                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Lecturers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId");

                    b.Navigation("Department");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Core.Entities.Topic", b =>
                {
                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Topics")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Group", "Group")
                        .WithMany("Topics")
                        .HasForeignKey("GroupId");

                    b.HasOne("Core.Entities.Lecturer", "Lecturer")
                        .WithMany("Topics")
                        .HasForeignKey("LecturerId");

                    b.HasOne("Core.Entities.Process", "Process")
                        .WithMany("Topics")
                        .HasForeignKey("ProcessId");

                    b.HasOne("Core.Entities.Status", "Status")
                        .WithMany("Topics")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Group");

                    b.Navigation("Lecturer");

                    b.Navigation("Process");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Core.Entities.Department", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Lecturers");

                    b.Navigation("Students");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Core.Entities.Group", b =>
                {
                    b.Navigation("Students");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Core.Entities.Lecturer", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Core.Entities.Process", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Core.Entities.Status", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Core.Entities.TypeAccount", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
