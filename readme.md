# Made by me
- All section of Partners

# SkillHub Web Project

## Description

This project was developed as part of the Professional Aptitude Test (PAP) for the Técnico de Gestão e Programação de Sistemas Informáticos course. The main objective was to create a web application named SkillHub, utilizing C# and SQL, based on the Blazor Server App architecture with MVVM (Model, View, ViewModel) in the .NET Core 6 environment.

The project addresses various aspects of Human Resources Management, including Absence+ (Absence Management), Partner+ (Partner Management), Timesheet+ (Time Tracking Management), and Collaborator+ (Employee Management). The primary focus of this project was on Partner Management (Partner+).

## Authentication and Security

The SkillHub application incorporates an OAuth-based login system, allowing authentication only through Microsoft accounts. The authentication is integrated with Azure AD to obtain user permissions (User, Supervisor, or Unauthorized). Access to partner-related resources is restricted to administrators (supervisors), preventing regular users from viewing partner-specific information.

The project's structure follows a comprehensive security approach, where communication between the front-end and the database is protected. The front-end communicates with an AppRepository, which in turn communicates with a Proxy generated based on the API. The Proxy establishes communication with the API, which communicates with a Repository. The Repository, finally, interacts with the Database, generated in a code-first mode.

It's crucial to note that at each stage of communication (e.g., from AppRepository to Proxy), data models need to be converted using Mappers (extension methods) to transform ContactModel into ApiContactModel and vice versa.

## Development Stage

### Main Hub - Innovation, Incubation & Development, lda

Main Hub is a Portuguese IT services and consulting company with over 9 years of experience. Specializing in Cloud Computing, Internet of Things, Big Data, and Machine Learning, they provide custom and high-quality solutions in development, outsourcing, and training. Main Hub also acts as an incubator for small businesses, assisting them in expanding their operations.

## Development Team

During the internship period, I collaborated with my course colleagues (Maksym Grebeniuk and Bruno Ideias) and another intern (Dinis Godinho) on a project aimed at solving the Human Resources section of a company. The project's goals included developing four aspects: Absence Management (Absence+), Partner Management (Partner+), Time Tracking Management (Timesheet+), and Employee Management (Collaborator+). The primary focus was on Partner Management (Partner+).

To ensure the application's data security and integrity, we implemented an OAuth-based authentication system. This allowed users to log in using their Microsoft accounts. Additionally, authentication was integrated with Azure AD to obtain the necessary permissions, differentiating users between User, Supervisor, or Unauthorized. Access to partner-related resources was restricted to supervisors, preventing regular users from accessing confidential information.

## Project Structure

The project's structure was planned to ensure security at every stage of communication between the front-end and the database. Communication was established through a flow involving the front-end, AppRepository, Proxy generated based on the API, Repository, and, finally, the Database. Each stage required data model conversion using Mappers (extension methods) to ensure the correct transformation of models between different layers.

During my internship, I received support and guidance from my mentor, Nuno Cancelo, who played a crucial role in my experience. He provided necessary support in terms of equipment and technical information for project task execution. Nuno was also responsible for supervising and assigning tasks, ensuring project requirements were met.

I am grateful for the experience and the opportunity to participate in this project, gaining new knowledge and insights both professionally and personally.

## Programs Used

- **Visual Studio**
  - Edition: Community 2022
  - Version: 17.6.4

- **.NET Core**
  - Version: SDK 6.0.100

- **Blazor**
  - Edition: Blazor Server App
  - Version: .NET Core 6

- **SQL Server Management Studio**
  - Edition: Management Studio 2019
  - Version: 19.1

- **Fork**
  - Version: 19.1

- **GIT**
  - Version: 2.41.0

- **GitHub Desktop**
  - Version: 3.2.3

 ## Resources
 - [Feature flags](https://learn.microsoft.com/en-us/dotnet/architecture/cloud-native/feature-flags)
 - [Tutorial: Use feature flags in an ASP.NET Core app](https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core)
 - [Build Validation](https://learn.microsoft.com/en-us/azure/devops/repos/git/branch-policies?view=azure-devops&tabs=browser#build-validation)
   
 ##Configuration
 In the folder 'infra' there are three pipelines: (Deleted to protect the company)
 - ci-pipeline.yaml
 - commit-ci-pipeline.yaml
 - daily-analisys-pipeline.yaml

  ### ci-pipeline.yaml 
  This pipeline is to be configured to be used on merge by pr on "relevant" branches.

  ### commit-ci-pipeline.yaml 
  This pipeline is to be configured to be used as a "Build Validation" policy on the target branch. It will be executed on a PR dependency.

  ### daily-analisys-pipeline.yaml 
  Runs every day at midnight. It will do Snyk, Withsource and Depency-Check analisys.
