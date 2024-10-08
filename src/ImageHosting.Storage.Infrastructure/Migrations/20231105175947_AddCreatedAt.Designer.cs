﻿// <auto-generated />
using System;
using ImageHosting.Storage.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ImageHosting.Storage.Migrations
{
    [DbContext(typeof(ImageHostingDbContext))]
    [Migration("20231105175947_AddCreatedAt")]
    partial class AddCreatedAt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Forbidden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ObjectName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.ImageCategory", b =>
                {
                    b.Property<Guid>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("ImageId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ImageCategories");
                });

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.ImageCategory", b =>
                {
                    b.HasOne("ImageHosting.Storage.WebApi.Entities.Category", "Category")
                        .WithMany("Categories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImageHosting.Storage.WebApi.Entities.Image", "Image")
                        .WithMany("Categories")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.Category", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("ImageHosting.Storage.WebApi.Entities.Image", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
