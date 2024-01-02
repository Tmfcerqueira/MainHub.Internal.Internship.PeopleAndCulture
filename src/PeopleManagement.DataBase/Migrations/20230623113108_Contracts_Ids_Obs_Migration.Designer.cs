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
    [Migration("20230623113108_Contracts_Ids_Obs_Migration")]
    partial class Contracts_Ids_Obs_Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.CollaboratorHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Action")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("None");

                    b.Property<DateTime>("ActionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("Adress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No_Adress");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("CCNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CCVal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ChangeDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("ChangedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No Change Date");

                    b.Property<string>("CivilState")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCivilState");

                    b.Property<int>("CollaboratorId")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int>("ContractType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("Country")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCountry");

                    b.Property<string>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No User");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DependentNum")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("noemail@mainhub.pt");

                    b.Property<DateTime>("EntryDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ExitDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoName");

                    b.Property<string>("Iban")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoName");

                    b.Property<string>("Locality")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoPostal_Code");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PeopleGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Postal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoPostal_Code");

                    b.Property<string>("SSNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Active");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Collaborator_CollaboratorId");

                    b.ToTable("PersonHistory");
                });

            modelBuilder.Entity("PeopleManagementDataBase.Collaborator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Adress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No_Adress");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("CCNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CCVal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ChangeDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("ChangedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No Change Date");

                    b.Property<string>("CivilState")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCivilState");

                    b.Property<int>("CollaboratorId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5)
                        .HasColumnType("int")
                        .HasDefaultValue(10000);

                    b.Property<int>("ContractType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("Country")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoCountry");

                    b.Property<string>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No User");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("DeletedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("No User");

                    b.Property<DateTime>("DeletedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<int>("DependentNum")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("noemail@mainhub.pt");

                    b.Property<DateTime>("EntryDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ExitDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoName");

                    b.Property<string>("Iban")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoName");

                    b.Property<string>("Locality")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoPostal_Code");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PeopleGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Postal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("NoPostal_Code");

                    b.Property<string>("SSNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Active");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Collaborator_Id");

                    b.HasAlternateKey("PeopleGUID")
                        .HasName("People_PeopleGUID");

                    b.ToTable("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
