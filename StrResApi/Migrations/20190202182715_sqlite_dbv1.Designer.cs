﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StrResData.Entities;

namespace StrResApi.Migrations
{
    [DbContext(typeof(StrResDbContext))]
    [Migration("20190202182715_sqlite_dbv1")]
    partial class sqlite_dbv1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("StrResData.Entities.Admin", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime>("ModifiedTime");

                    b.HasKey("Name");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("StrResData.Entities.Resource", b =>
                {
                    b.Property<long>("TenantId");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("TenantId", "Key");

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("StrResData.Entities.Tenant", b =>
                {
                    b.Property<long>("TenantId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("TenantId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tenant");
                });

            modelBuilder.Entity("StrResData.Entities.Resource", b =>
                {
                    b.HasOne("StrResData.Entities.Tenant", "Tenant")
                        .WithMany("Resources")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}