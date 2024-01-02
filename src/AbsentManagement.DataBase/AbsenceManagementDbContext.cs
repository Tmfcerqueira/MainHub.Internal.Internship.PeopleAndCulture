using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using PeopleManagementDataBase;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;

public partial class AbsenceManagementDbContext : DbContext
{

    public virtual DbSet<Absence> Absence { get; set; }

    public virtual DbSet<AbsenceType> AbsenceType { get; set; }
    public virtual DbSet<AbsenceHistory> AbsenceHistory { get; set; }
    public virtual DbSet<AbsenceTypeHistory> AbsenceTypeHistory { get; set; }
    public virtual DbSet<Collaborator> Person { get; set; } = null!;


    public AbsenceManagementDbContext(DbContextOptions<AbsenceManagementDbContext> options) : base(options)
    {
        Absence = Set<Absence>();
        AbsenceType = Set<AbsenceType>();
        AbsenceHistory = Set<AbsenceHistory>();
        AbsenceTypeHistory = Set<AbsenceTypeHistory>();
    }


    //relations, keys, required propreties
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configure Absence entity

        //Db Id
        modelBuilder.Entity<Absence>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd().UseIdentityColumn();

        modelBuilder.Entity<Absence>()
            .HasKey(p => p.Id)
            .HasName("PK_Absence_Id");


        //Guid Id

        modelBuilder.Entity<Absence>()
            .Property(p => p.AbsenceGuid)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Absence>()
            .HasAlternateKey(p => p.AbsenceGuid)
            .HasName("AK_Absence_Guid");

        //Other propreties

        modelBuilder.Entity<Absence>(entity =>
        {

            entity.Property(e => e.PersonGuid)
           .IsRequired()
           .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.AbsenceTypeGuid)
            .IsRequired()
            .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.AbsenceStart)
            .IsRequired()
            .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.AbsenceEnd)
            .IsRequired()
            .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.ApprovalStatus)
            .IsRequired()
            .HasDefaultValue(ApprovalStatus.Draft);

            entity.Property(e => e.SubmissionDate)
            .IsRequired()
            .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.Schedule)
           .IsRequired()
           .HasDefaultValue("Full Day");

            entity.Property(e => e.ApprovedBy)
          .IsRequired()
          .HasDefaultValue("None");

            entity.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

            entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.DeletedBy)
            .IsRequired()
            .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.Description)
            .IsRequired()
            .HasDefaultValue("None");


        });

        //configure AbsenceType entity

        //Db Id
        modelBuilder.Entity<AbsenceType>()
            .Property(p => p.Id).ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<AbsenceType>()
            .HasKey(p => p.Id)
            .HasName("PK_AbsenceType_Id");


        //Guid Id

        modelBuilder.Entity<AbsenceType>()
            .Property(p => p.TypeGuid)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<AbsenceType>()
            .HasAlternateKey(p => p.TypeGuid)
            .HasName("AK_AbsenceType_Guid");

        //Other Proprieties

        modelBuilder.Entity<AbsenceType>()
            .Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue("Has no Type");

        modelBuilder.Entity<AbsenceType>()
            .Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        modelBuilder.Entity<AbsenceType>()
           .Property(e => e.CreatedBy)
           .IsRequired()
           .HasDefaultValue(Guid.Empty);

        modelBuilder.Entity<AbsenceType>()
           .Property(e => e.ModifiedBy)
           .IsRequired()
           .HasDefaultValue(Guid.Empty);

        modelBuilder.Entity<AbsenceType>()
           .Property(e => e.DeletedBy)
           .IsRequired()
           .HasDefaultValue(Guid.Empty);


        //Relations

        //Absence - AbsenceType

        modelBuilder.Entity<AbsenceType>()
           .HasMany(aType => aType.Absences)
           .WithOne(a => a.AbsenceType)
           .HasForeignKey(aType => aType.AbsenceTypeGuid)
           .HasPrincipalKey(aType => aType.TypeGuid);



        // configure AbsenceHistory entity

        // Db Id
        modelBuilder.Entity<AbsenceHistory>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd().UseIdentityColumn();

        modelBuilder.Entity<AbsenceHistory>()
            .HasKey(p => p.Id)
            .HasName("PK_AbsenceHistory_Id");

        // Guid Id
        modelBuilder.Entity<AbsenceHistory>()
            .Property(p => p.AbsenceGuid)
            .IsRequired();

        // Other properties
        modelBuilder.Entity<AbsenceHistory>(entity =>
        {
            entity.Property(e => e.PersonGuid)
                .IsRequired()
                .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.AbsenceTypeGuid)
                .IsRequired()
                .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.AbsenceStart)
                .IsRequired()
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.AbsenceEnd)
                .IsRequired()
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.ApprovalStatus)
                .IsRequired()
                .HasDefaultValue(ApprovalStatus.Draft);

            entity.Property(e => e.ApprovedBy)
            .IsRequired()
            .HasDefaultValue("None");

            entity.Property(e => e.ApprovalStatus)
                .IsRequired()
                .HasDefaultValue(ApprovalStatus.Draft);


            entity.Property(e => e.SubmissionDate)
                .IsRequired()
                .HasDefaultValue(new DateTime(2999, 12, 31, 23, 59, 59));

            entity.Property(e => e.Schedule)
                .IsRequired()
                .HasDefaultValue("Full Day");

            entity.Property(e => e.ActionText)
                .IsRequired()
                .HasDefaultValue("None");

            entity.Property(e => e.ActionDate)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasDefaultValue(Guid.Empty);

            entity.Property(e => e.Description)
            .IsRequired()
            .HasDefaultValue("None");
        });

        // configure AbsenceTypeHistory entity

        // Db Id
        modelBuilder.Entity<AbsenceTypeHistory>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<AbsenceTypeHistory>()
            .HasKey(p => p.Id)
            .HasName("PK_AbsenceTypeHistory_Id");

        // Guid Id
        modelBuilder.Entity<AbsenceTypeHistory>()
            .Property(p => p.TypeGuid)
            .IsRequired();

        // Other Properties
        modelBuilder.Entity<AbsenceTypeHistory>()
            .Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue("Has no Type");

        modelBuilder.Entity<AbsenceTypeHistory>()
            .Property(e => e.ActionText)
            .IsRequired()
            .HasDefaultValue("None");

        modelBuilder.Entity<AbsenceTypeHistory>()
            .Property(e => e.ActionDate)
            .IsRequired()
            .HasDefaultValue(DateTime.Now);

        modelBuilder.Entity<AbsenceTypeHistory>()
           .Property(e => e.UserId)
           .IsRequired()
           .HasDefaultValue(Guid.Empty);
    }

}



