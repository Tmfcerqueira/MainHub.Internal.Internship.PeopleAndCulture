﻿// <auto-generated />
using System;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    [DbContext(typeof(AbsenceManagementDbContext))]
    [Migration("20230421091715_AbsenceAddSchedule")]
    partial class AbsenceAddSchedule
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models.Absence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AbsenceEnd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<Guid>("AbsenceGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AbsenceStart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<Guid>("AbsenceTypeGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("ApprovedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PersonGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<int>("Schedule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("SubmissionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.HasKey("Id")
                        .HasName("PK_Absence_Id");

                    b.HasAlternateKey("AbsenceGuid")
                        .HasName("AK_Absence_Guid");

                    b.HasIndex("AbsenceTypeGuid");

                    b.ToTable("Absence");
                });

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models.AbsenceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Has no Type");

                    b.Property<Guid>("TypeGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK_AbsenceType_Id");

                    b.HasAlternateKey("TypeGuid")
                        .HasName("AK_AbsenceType_Guid");

                    b.ToTable("AbsenceType");
                });

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models.Absence", b =>
                {
                    b.HasOne("MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models.AbsenceType", "AbsenceType")
                        .WithMany("Absences")
                        .HasForeignKey("AbsenceTypeGuid")
                        .HasPrincipalKey("TypeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AbsenceType");
                });

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models.AbsenceType", b =>
                {
                    b.Navigation("Absences");
                });
#pragma warning restore 612, 618
        }
    }
}
