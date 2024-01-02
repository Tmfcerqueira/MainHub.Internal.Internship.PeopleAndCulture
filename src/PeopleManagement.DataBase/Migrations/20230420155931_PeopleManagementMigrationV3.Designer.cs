﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeopleManagementRepository.Data;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    [DbContext(typeof(PeopleManagementDbContext))]
    [Migration("20230420155931_PeopleManagementMigrationV3")]
    partial class PeopleManagementMigrationV3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PeopleManagementDataBase.Collaborator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Adress")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No_Adress");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("CCNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CCVal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ChangeDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No Change Date");

                    b.Property<string>("CivilState")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCivilState");

                    b.Property<string>("Country")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCountry");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No User");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<int>("DependentNum")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("EntryDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ExitDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoName");

                    b.Property<Guid>("PeopleGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Postal")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoPostal_Code");

                    b.Property<string>("SSNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Active");

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("Collaborator_Id");

                    b.HasAlternateKey("CCNumber");

                    b.HasAlternateKey("PeopleGUID")
                        .HasName("People_PeopleGUID");

                    b.HasAlternateKey("SSNumber");

                    b.HasAlternateKey("TaxNumber");

                    b.ToTable("Person");
                });
#pragma warning restore 612, 618
        }
    }
}