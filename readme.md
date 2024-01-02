# Resources
- [Feature flags](https://learn.microsoft.com/en-us/dotnet/architecture/cloud-native/feature-flags)
- [Tutorial: Use feature flags in an ASP.NET Core app](https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core)
- [Build Validation](https://learn.microsoft.com/en-us/azure/devops/repos/git/branch-policies?view=azure-devops&tabs=browser#build-validation)
# Configuration
In the folder 'infra' there are three pipelines: (Deleted to protect the company)
- ci-pipeline.yaml
- commit-ci-pipeline.yaml
- daily-analisys-pipeline.yaml

 ## ci-pipeline.yaml (Deleted to protect the company)
 This pipeline is to be configured to be used on merge by pr on "relevant" branches.

 ## commit-ci-pipeline.yaml (Deleted to protect the company)
 This pipeline is to be configured to be used as a "Build Validation" policy on the target branch. It will be executed on a PR dependency.

 ## daily-analisys-pipeline.yaml (Deleted to protect the company)
 Runs every day at midnight. It will do Snyk, Withsource and Depency-Check analisys.
