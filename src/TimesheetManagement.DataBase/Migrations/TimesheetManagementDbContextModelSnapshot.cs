﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimesheetManagement.DataBase;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    [DbContext(typeof(TimesheetManagementDbContext))]
    partial class TimesheetManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid>("UserGUID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TimesheetActivityId")
                        .HasName("PK_TimesheetActivityHistory_ActivityId");

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

                    b.Property<Guid>("UserGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("TimesheetId")
                        .HasName("PK_TimesheetHistory_TimesheetId");

                    b.ToTable("TimesheetHistory");
                });

            modelBuilder.Entity("PeopleManagementDataBase.Collaborator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CCNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CCVal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChangedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CivilState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DependentNum")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExitDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PeopleGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Postal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SSNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Person");
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

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfApproval")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<DateTime>("DateOfSubmission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

                    b.Property<Guid>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EditedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

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

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EditedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Hours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("ProjectGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

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

                    b.HasIndex("ProjectId");

                    b.HasIndex("TimesheetGUID");

                    b.ToTable("TimesheetActivity");
                });

            modelBuilder.Entity("TimesheetManagement.DataBase.Models.TimesheetActivity", b =>
                {
                    b.HasOne("TimesheetManagement.DataBase.Models.Project", "Project")
                        .WithMany("TimesheetActivities")
                        .HasForeignKey("ProjectId");

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
