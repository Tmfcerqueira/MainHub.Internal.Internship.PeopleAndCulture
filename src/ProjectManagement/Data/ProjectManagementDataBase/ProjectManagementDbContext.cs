
using MainHub.Internal.PeopleAndCulture.ProjectManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Data
{
    public class ProjectManagementDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }

        public ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options) : base(options)
        {
            Projects = Set<Project>();
            ProjectActivities = Set<ProjectActivity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreatingV0(modelBuilder);
            ModelCreatingV0_Seed(modelBuilder);

        }

        private void ModelCreatingV0_Seed(ModelBuilder modelBuilder)
        {
            ProjectModelCreating_Seed(modelBuilder);
            ProjectActivityModelCreating_Seed(modelBuilder);
        }

        private void ProjectActivityModelCreating_Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectActivity>()
                .HasData(
                    new ProjectActivity { Id = 1, Name = "Desenvolvimento" },
                    new ProjectActivity { Id = 2, Name = "Análise Processo" },
                    new ProjectActivity { Id = 3, Name = "Análise Técnica" },
                    new ProjectActivity { Id = 4, Name = "Documentação" },
                    new ProjectActivity { Id = 5, Name = "Formação" },
                    new ProjectActivity { Id = 6, Name = "Reuniões" },
                    new ProjectActivity { Id = 7, Name = "Férias" },
                    new ProjectActivity { Id = 8, Name = "Consulta Médica" },
                    new ProjectActivity { Id = 9, Name = "Outro" }
                );
        }

        private void ProjectModelCreating_Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasData(
                    new Project { Id = 1, Name = "Singular Data" },
                    new Project { Id = 2, Name = "GS Tyres" },
                    new Project { Id = 3, Name = "Flumen" },
                    new Project { Id = 4, Name = "Grupo Pestana" },
                    new Project { Id = 5, Name = "CCP" },
                    new Project { Id = 6, Name = "Milestone" },
                    new Project { Id = 7, Name = "AdvanceCare" },
                    new Project { Id = 8, Name = "EY - Turismo de Portugal" },
                    new Project { Id = 9, Name = "EY - Abreu Advogados" }
                );
        }

        private static void ModelCreatingV0(ModelBuilder modelBuilder)
        {
            ProjectModelCreating(modelBuilder);
            ProjectActivityModelCreating(modelBuilder);
        }

        private static void ProjectActivityModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectActivity>()
                            .Property(t => t.Id)
                            .ValueGeneratedOnAdd()
                            .UseIdentityColumn();

            modelBuilder.Entity<ProjectActivity>()
                .HasKey(t => t.Id)
                .HasName("PK_ProjectActivity_Id");

            modelBuilder.Entity<ProjectActivity>()
                .HasAlternateKey(t => t.ProjectActivityId)
                .HasName("AK_ProjectActivity_ProjectActivityId");

            modelBuilder.Entity<ProjectActivity>()
                .Property(t => t.ProjectActivityId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProjectActivity>()
                .Property(t => t.Name)
                .IsRequired()
                .HasDefaultValue("No Project Ativity");
        }
        private static void ProjectModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                            .Property(t => t.Id)
                            .ValueGeneratedOnAdd()
                            .UseIdentityColumn();

            modelBuilder.Entity<Project>()
                .HasKey(t => t.Id)
                .HasName("PK_Project_Id");

            modelBuilder.Entity<Project>()
                .HasAlternateKey(t => t.ProjectId)
                .HasName("AK_Project_ProjectId");

            modelBuilder.Entity<Project>()
                .Property(t => t.ProjectId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Project>()
                .Property(t => t.Name)
                .IsRequired()
                .HasDefaultValue("No Project");
        }
    }
}
