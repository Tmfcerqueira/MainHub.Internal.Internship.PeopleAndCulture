using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartnerManagement.DataBase.Models;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using MainHub.Internal.PeopleAndCulture.Models;

namespace PartnerManagement.DataBase
{
    public class PartnerManagementDBContext : DbContext
    {
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<PartnerHistory> PartnerHistory { get; set; }
        public virtual DbSet<ContactHistory> ContactHistory { get; set; }

        public PartnerManagementDBContext(DbContextOptions<PartnerManagementDBContext> options)
            : base(options)
        {
            Partner = Set<Partner>();
            Contact = Set<Contact>();
            PartnerHistory = Set<PartnerHistory>();
            ContactHistory = Set<ContactHistory>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //**************************************************
            //*              Partner Cnf_Entity                *
            //**************************************************

            modelBuilder.Entity<Partner>()
                .HasKey(p => p.Id)
                .HasName("PK_Partner_ID");

            modelBuilder.Entity<Partner>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.PartnerGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .HasAlternateKey(p => p.PartnerGUID)
                .HasName("AK_Partner_Guid");

            modelBuilder.Entity<Partner>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Partner>()
            .Property(p => p.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.Address)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.Locality)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.Country)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.TaxNumber)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.ServiceDescription)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.Observation)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.CreationDate)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.ChangedDate)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.ModifiedBy)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.State)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Partner>()
                .Property(p => p.DeletedBy)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Partner>()
                .ToTable("Partner");


            //**************************************************
            //*                Partner History                 *
            //**************************************************

            // Configure Partner entity
            modelBuilder.Entity<PartnerHistory>()
                .HasKey(p => p.Id)
                .HasName("PK_PartnerHistory_ID");

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.PartnerGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
            .Property(p => p.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Address)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Locality)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Country)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.TaxNumber)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.ServiceDescription)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Observation)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.CreationDate)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.ChangedDate)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.ModifiedBy)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.State)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.Action)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
               .Property(p => p.ActionDate)
               .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
                .Property(p => p.UserGUID)
                .IsRequired();

            modelBuilder.Entity<PartnerHistory>()
             .ToTable("Partner_History");

            //**************************************************
            //*              Contact Cnf_Entity                *
            //**************************************************


            modelBuilder.Entity<Contact>()
                    .HasKey(pcl => pcl.Id)
                    .HasName("PK_Contact_ID");

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.ContactGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .HasAlternateKey(pcl => pcl.ContactGUID)
                .HasName("AK_Contact_Guid");

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.PartnerGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Role)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Department)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.Observation)
                .HasMaxLength(255).IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.DeletedBy)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(pcl => pcl.UserGUID)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .ToTable("Contact");

            modelBuilder.Entity<Partner>()
                .HasMany(p => p.Contact)
                .WithOne(pcl => pcl.Partner)
                .HasForeignKey(pcl => pcl.PartnerGUID)
                .HasPrincipalKey(p => p.PartnerGUID);


            //**************************************************
            //*                Contact History                 *
            //**************************************************


            modelBuilder.Entity<ContactHistory>()
        .HasKey(pcl => pcl.Id)
        .HasName("PK_ContactHistory_ID");

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.ContactGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.PartnerGUID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.Role)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(pcl => pcl.Department)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
       .Property(pcl => pcl.Observation)
       .HasMaxLength(255).IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(p => p.Action)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
               .Property(p => p.ActionDate)
               .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .Property(p => p.UserGUID)
                .IsRequired();

            modelBuilder.Entity<ContactHistory>()
                .ToTable("Contact_History");
        }
    }
}
