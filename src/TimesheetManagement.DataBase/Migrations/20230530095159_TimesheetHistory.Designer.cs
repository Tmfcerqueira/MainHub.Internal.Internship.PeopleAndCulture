﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimesheetManagement.DataBase;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    [DbContext(typeof(TimesheetManagementDbContext))]
    [Migration("20230530095159_TimesheetHistory")]
    partial class TimesheetHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.Models.TimesheetActivityHistory", b =>
                {
                    b.Property<int>("TimesheetActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetActivityId"), 1L, 1);

                    b.Property<string>("Action")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasDefaultValue("None");

                    b.Property<DateTime>("ActionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("ActivityDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<Guid>("ActivityGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Hours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("ProjectGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetActivityGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TypeOfWork")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32)
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("TimesheetActivityId")
                        .HasName("PK_TimesheetActivityHistory_ActivityId");

                    b.HasAlternateKey("TimesheetActivityGUID")
                        .HasName("AK_TimesheetActivityHistory_ActivityGuid");

                    b.ToTable("TimesheetActivityHistory");
                });

            modelBuilder.Entity("MainHub.Internal.PeopleAndCulture.Models.TimesheetHistory", b =>
                {
                    b.Property<int>("TimesheetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetId"), 1L, 1);

                    b.Property<string>("Action")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasDefaultValue("None");

                    b.Property<DateTime>("ActionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<int>("ApprovalStatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("ApprovedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValue("None");

                    b.Property<DateTime>("DateOfApproval")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("DateOfSubmission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<int>("Month")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("PersonGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("TimesheetId")
                        .HasName("PK_TimesheetHistory_TimesheetId");

                    b.HasAlternateKey("TimesheetGUID")
                        .HasName("AK_TimesheetHistory_TimesheetGuid");

                    b.ToTable("TimesheetHistory");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)")
                        .HasDefaultValue("None");

                    b.Property<Guid>("ProjectGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProjectId")
                        .HasName("PK_Project_ProjectId");

                    b.HasAlternateKey("ProjectGUID")
                        .HasName("AK_Project_ProjectGuid");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.Timesheet", b =>
                {
                    b.Property<int>("TimesheetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetId"), 1L, 1);

                    b.Property<int>("ApprovalStatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("ApprovedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValue("None");

                    b.Property<DateTime>("DateOfApproval")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("DateOfSubmission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bool")
                        .HasDefaultValue(false);

                    b.Property<int>("Month")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("PersonGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("TimesheetId")
                        .HasName("PK_Timesheet_TimesheetId");

                    b.HasAlternateKey("TimesheetGUID")
                        .HasName("AK_Timesheet_TimesheetGuid");

                    b.ToTable("Timesheet");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.TimesheetActivity", b =>
                {
                    b.Property<int>("TimesheetActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetActivityId"), 1L, 1);

                    b.Property<DateTime>("ActivityDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<Guid>("ActivityGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Hours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bool")
                        .HasDefaultValue(false);

                    b.Property<Guid>("ProjectGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetActivityGUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TimesheetGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TypeOfWork")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32)
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("TimesheetActivityId")
                        .HasName("PK_TimesheetActivity_ActivityId");

                    b.HasAlternateKey("TimesheetActivityGUID")
                        .HasName("AK_TimesheetActivity_ActivityGuid");

                    b.HasIndex("ProjectGUID");

                    b.HasIndex("TimesheetGUID");

                    b.ToTable("TimesheetActivity");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.TimesheetActivity", b =>
                {
                    b.HasOne("TimesheetManagement.DataBase.Models.Project", "Project")
                        .WithMany("TimesheetActivities")
                        .HasForeignKey("ProjectGUID")
                        .HasPrincipalKey("ProjectGUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TimesheetActivity_ProjectGuid");

                    b.HasOne("TimesheetManagement.DataBase.Models.Timesheet", "Timesheet")
                        .WithMany("TimesheetActivities")
                        .HasForeignKey("TimesheetGUID")
                        .HasPrincipalKey("TimesheetGUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TimesheetActivity_TimesheetGuid");

                    b.Navigation("Project");

                    b.Navigation("Timesheet");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.Project", b =>
                {
                    b.Navigation("TimesheetActivities");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.Timesheet", b =>
                {
                    b.Navigation("TimesheetActivities");
                });
#pragma warning restore 612, 618
        }
    }
}
