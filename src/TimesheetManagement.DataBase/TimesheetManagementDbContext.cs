using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PeopleManagementDataBase;
using TimesheetManagement.DataBase.Models;

namespace TimesheetManagement.DataBase
{
    public partial class TimesheetManagementDbContext : DbContext
    {
        public virtual DbSet<Timesheet> Timesheet { get; set; }
        public virtual DbSet<TimesheetHistory> TimesheetHistory { get; set; }
        public virtual DbSet<TimesheetActivity> TimesheetActivity { get; set; }
        public virtual DbSet<TimesheetActivityHistory> TimesheetActivityHistory { get; set; }
        public virtual DbSet<Collaborator> Person { get; set; } = null!;

        public TimesheetManagementDbContext(DbContextOptions<TimesheetManagementDbContext> options) : base(options)
        {
            Timesheet = Set<Timesheet>();
            TimesheetHistory = Set<TimesheetHistory>();
            TimesheetActivity = Set<TimesheetActivity>();
            TimesheetActivityHistory = Set<TimesheetActivityHistory>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Table Timesheet
            modelBuilder.Entity<Timesheet>()
                .Property(t => t.TimesheetId)
                .ValueGeneratedOnAdd().UseIdentityColumn();

            modelBuilder.Entity<Timesheet>()
                .HasKey(t => t.TimesheetId) //Set TimesheetId as the primary key
                .HasName("PK_Timesheet_TimesheetId");

            modelBuilder.Entity<Timesheet>()
                .HasAlternateKey(t => t.TimesheetGUID)
                .HasName("AK_Timesheet_TimesheetGuid");

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.TimesheetGUID)
                .ValueGeneratedOnAdd(); //Set TimesheetGUID to be generated with new GUIDs

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.PersonGUID)
                .IsRequired(); //Set PersonGUID as a required field

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.Month)
                .IsRequired() //Set Month as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.Year)
                .IsRequired() //Set Year as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.ApprovalStatus)
                .HasMaxLength(50) //Set ApprovalState to have a maximum length of 50 characters
                .HasDefaultValue(ApprovalStatus.Draft);

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.ApprovedBy)
                .HasMaxLength(100) //Set ApprovedBy to have a maximum length of 100 characters
                .HasDefaultValue("None");

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.DateOfSubmission)
                .HasColumnType("datetime") //Set DateOfSubmission to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.DateOfApproval)
                .HasColumnType("datetime") //Set DateOfApproval to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.IsDeleted)
                .IsRequired() //Set IsDeleted as a required field
                .HasDefaultValue(false)
                .HasConversion<int>();

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.EditedBy)
                .IsRequired();

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.DeletedBy)
                .IsRequired();



            //Table TimesheetHistory
            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.TimesheetId)
                .ValueGeneratedOnAdd().UseIdentityColumn();

            modelBuilder.Entity<TimesheetHistory>()
                .HasKey(t => t.TimesheetId) //Set TimesheetId as the primary key
                .HasName("PK_TimesheetHistory_TimesheetId");

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.TimesheetGUID)
                .ValueGeneratedOnAdd(); //Set TimesheetGUID to be generated with new GUIDs

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.PersonGUID)
                .IsRequired(); //Set PersonGUID as a required field

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.Month)
                .IsRequired() //Set Month as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.Year)
                .IsRequired() //Set Year as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.ApprovalStatus)
                .HasMaxLength(50) //Set ApprovalState to have a maximum length of 50 characters
                .HasDefaultValue(ApprovalStatus.Draft);

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.ApprovedBy)
                .HasMaxLength(100) //Set ApprovedBy to have a maximum length of 100 characters
                .HasDefaultValue("None");

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.DateOfSubmission)
                .HasColumnType("datetime") //Set DateOfSubmission to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.DateOfApproval)
                .HasColumnType("datetime") //Set DateOfApproval to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.Action)
                .HasMaxLength(32) //Set Action to have a maximum length of 32 characters
                .HasDefaultValue("None");

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.ActionDate)
                .HasColumnType("datetime") //Set ActionDate to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetHistory>()
                .Property(t => t.UserGUID)
                .IsRequired(); //Set UserGUID as a required field



            //Table TimesheetActivity
            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.TimesheetActivityId)
                .ValueGeneratedOnAdd().UseIdentityColumn();

            modelBuilder.Entity<TimesheetActivity>()
                .HasKey(a => a.TimesheetActivityId) //Set TimesheetActivityId as the primary key
                .HasName("PK_TimesheetActivity_ActivityId");

            modelBuilder.Entity<TimesheetActivity>()
                .HasAlternateKey(a => a.TimesheetActivityGUID)
                .HasName("AK_TimesheetActivity_ActivityGuid");

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.TimesheetActivityGUID)
                .ValueGeneratedOnAdd(); //Set TimesheetActivityGUID to be generated with new GUIDs

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.ActivityGUID)
                .IsRequired(); //Set ActivityGUID as a required field

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.TimesheetGUID)
                .IsRequired(); //Set TimesheetGUID as a required field

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.ProjectGUID)
                .IsRequired(); //Set ProjectGUID as a required field

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.TypeOfWork)
                .HasMaxLength(32) //Set TypeOfWork to have a maximum length of 32 characters
                .HasDefaultValue(TypeOfWork.Regular);

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.ActivityDate)
                .HasColumnType("datetime") //Set ActivityDate to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.Hours)
                .IsRequired() //Set Hours as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.IsDeleted)
                .IsRequired() //Set IsDeleted as a required field
                .HasDefaultValue(false)
                .HasConversion<int>();

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.EditedBy)
                .IsRequired();

            modelBuilder.Entity<TimesheetActivity>()
                .Property(a => a.DeletedBy)
                .IsRequired();



            //Table TimesheetActivityHistory
            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.TimesheetActivityId)
                .ValueGeneratedOnAdd().UseIdentityColumn();

            modelBuilder.Entity<TimesheetActivityHistory>()
                .HasKey(a => a.TimesheetActivityId) //Set TimesheetActivityId as the primary key
                .HasName("PK_TimesheetActivityHistory_ActivityId");

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.TimesheetActivityGUID)
                .ValueGeneratedOnAdd(); //Set TimesheetActivityGUID to be generated with new GUIDs

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.ActivityGUID)
                .IsRequired(); //Set ActivityGUID as a required field

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.TimesheetGUID)
                .IsRequired(); //Set TimesheetGUID as a required field

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.ProjectGUID)
                .IsRequired(); //Set ProjectGUID as a required field

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.TypeOfWork)
                .HasMaxLength(32) //Set TypeOfWork to have a maximum length of 32 characters
                .HasDefaultValue(TypeOfWork.Regular);

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.ActivityDate)
                .HasColumnType("datetime") //Set ActivityDate to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(a => a.Hours)
                .IsRequired() //Set Hours as a required field
                .HasDefaultValue(0);

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(t => t.Action)
                .HasMaxLength(32) //Set Action to have a maximum length of 32 characters
                .HasDefaultValue("None");

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(t => t.ActionDate)
                .HasColumnType("datetime") //Set ActionDate to be stored as a date type in the database
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            modelBuilder.Entity<TimesheetActivityHistory>()
                .Property(t => t.UserGUID)
                .IsRequired(); //Set UserGUID as a required field




            //Realations
            modelBuilder.Entity<Timesheet>()
                .HasMany(t => t.TimesheetActivities)
                .WithOne(a => a.Timesheet)
                .HasForeignKey(a => a.TimesheetGUID)
                .HasPrincipalKey(t => t.TimesheetGUID)
                .HasConstraintName("FK_TimesheetActivity_TimesheetGuid")
                .IsRequired();
        }
    }
}
