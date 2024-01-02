using MainHub.Internal.PeopleAndCulture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeopleManagementDataBase;

namespace PeopleManagementRepository.Data
{
    public class PeopleManagementDbContext : DbContext
    {
        public PeopleManagementDbContext(DbContextOptions<PeopleManagementDbContext> options) : base(options)
        {
        }
        public PeopleManagementDbContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Collaborator>()
                .HasKey(c => c.Id)
                .HasName("Collaborator_Id");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.PeopleGUID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Collaborator>()
                .HasAlternateKey(c => c.PeopleGUID)
                .HasName("People_PeopleGUID");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasDefaultValue("NoName");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasDefaultValue("NoName");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.BirthDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Adress)
                .HasDefaultValue("No_Adress");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Postal)
                .HasDefaultValue("NoPostal_Code");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Locality)
                .HasDefaultValue("NoPostal_Code");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Country)
                .HasDefaultValue("NoCountry");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.TaxNumber);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.CCNumber);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.SSNumber);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Iban);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.CCVal)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.CivilState)
                .HasDefaultValue("NoCivilState");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.DependentNum)
                .HasDefaultValue(0);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.EntryDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.ExitDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.CreationDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.CreatedBy)
                .HasDefaultValue("No User");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.ChangeDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.ChangedBy)
                .HasDefaultValue("No Change Date");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Status)
                .HasDefaultValue("Active");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.DeletedBy)
                .HasDefaultValue("No User");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.DeletedDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Email)
                .HasDefaultValue("noemail@mainhub.pt")
                .IsRequired();
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.ContractType)
                .HasDefaultValue(Contract.NoTerm);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Observations);
            modelBuilder.Entity<Collaborator>()
                .Property(c => c.Employee_Id)
                .HasMaxLength(3);
            modelBuilder.Entity<Collaborator>()
               .Property(c => c.Contact)
               .HasMaxLength(15);
            modelBuilder.Entity<Collaborator>()
               .Property(c => c.EmergencyContact)
               .HasMaxLength(15);
            //**************************************************
            //*                Person History                 *
            //**************************************************

            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<CollaboratorHistory>()
                .HasKey(c => c.Id)
                .HasName("Collaborator_CollaboratorId");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.PeopleGUID);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasDefaultValue("NoName");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasDefaultValue("NoName");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.BirthDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Adress)
                .HasDefaultValue("No_Adress");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Postal)
                .HasDefaultValue("NoPostal_Code");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Locality)
                .HasDefaultValue("NoPostal_Code");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Country)
                .HasDefaultValue("NoCountry");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.TaxNumber);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.CCNumber);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.SSNumber);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Iban);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.CCVal)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.CivilState)
                .HasDefaultValue("NoCivilState");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.DependentNum)
                .HasDefaultValue(0);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.EntryDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.ExitDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.CreationDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.CreatedBy)
                .HasDefaultValue("No User");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.ChangeDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.ChangedBy)
                .HasDefaultValue("No Change Date");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Status)
                .HasDefaultValue("Active");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.DeletedBy)
                .HasDefaultValue("No User");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.DeletedDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Action)
                .HasDefaultValue("None");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.ActionDate)
                .HasDefaultValue("2999-12-31 23:59:59");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.UserID)
                .IsRequired();
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Email)
                .IsRequired()
                .HasDefaultValue("noemail@mainhub.pt");
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.ContractType)
                .HasDefaultValue(Contract.NoTerm);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Employee_Id)
                .HasMaxLength(3);
            modelBuilder.Entity<CollaboratorHistory>()
                .Property(c => c.Observations);
            modelBuilder.Entity<CollaboratorHistory>()
               .Property(c => c.Contact)
               .HasMaxLength(15);
            modelBuilder.Entity<CollaboratorHistory>()
               .Property(c => c.EmergencyContact)
               .HasMaxLength(15);

        }
        public DbSet<Collaborator> Person { get; set; }
        public DbSet<CollaboratorHistory> PersonHistory { get; set; }
    }
}
