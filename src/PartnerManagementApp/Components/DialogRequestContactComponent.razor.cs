using System;
using System.Text.RegularExpressions;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.JSInterop;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Polly;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class DialogRequestContactComponent
    {
        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;
        private readonly Context? _editContext;
        [Parameter]
        public PartnerModel Partner { get; set; } = null!;

        private readonly PartnerApiPagination _partnerApiPagination = new();
        private ContactModel ContactModel { get; set; } = new ContactModel();
        [Inject]
        private GraphServiceClient GraphServiceClient { get; set; } = null!;

        protected User _user = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _user = await GraphServiceClient.Me.Request().GetAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task HandleValidSubmit()
        {
            try
            {
                if (ContactModel == null ||
                    (string.IsNullOrEmpty(ContactModel.Name) &&
                     string.IsNullOrEmpty(ContactModel.Email) &&
                     string.IsNullOrEmpty(ContactModel.Role) &&
                     string.IsNullOrEmpty(ContactModel.PhoneNumber) &&
                     string.IsNullOrEmpty(ContactModel.Department) &&
                     string.IsNullOrEmpty(ContactModel.Observation)))
                {
                    await DialogService.Alert($"{Localization["PleaseInsertRequestContact"]}", $"{Localization["Alert"]}");
                }
                else
                {
                    ContactModel.UserGUID = Guid.Parse(_user.Id);

                    List<string> missingFields = new List<string>();

                    if (string.IsNullOrEmpty(ContactModel.Name))
                    {
                        missingFields.Add(Localization["EnterName"]);
                    }

                    if (string.IsNullOrEmpty(ContactModel.Email))
                    {
                        missingFields.Add(Localization["EnterEmail"]);
                    }
                    else if (!VerifyEmail(ContactModel.Email))
                    {
                        await DialogService.Alert($"{Localization["InvalidEmail"]}", $"{Localization["Alert"]}");
                    }

                    if (string.IsNullOrEmpty(ContactModel.Role))
                    {
                        missingFields.Add(Localization["EnterRole"]);
                    }

                    if (string.IsNullOrEmpty(ContactModel.PhoneNumber))
                    {
                        missingFields.Add(Localization["EnterPhoneNumber"]);
                    }
                    else if (!VerifyPhoneNumber(ContactModel.PhoneNumber))
                    {
                        await DialogService.Alert($"{Localization["InvalidPhoneNumber"]}", $"{Localization["Alert"]}");
                    }

                    if (string.IsNullOrEmpty(ContactModel.Department))
                    {
                        missingFields.Add(Localization["EnterDepartment"]);
                    }

                    if (string.IsNullOrEmpty(ContactModel.Observation))
                    {
                        missingFields.Add(Localization["EnterObservation"]);
                    }

                    if (missingFields.Count > 0)
                    {
                        string missingFieldsList = string.Join($" {Localization["And"]} ", missingFields);
                        await DialogService.Alert($"{missingFieldsList} {Localization["Missing"]}, {Localization["Please"]}", $"{Localization["Alert"]}");
                    }
                    else
                    {
                        var result = await PartnerRepository.App_Create_Contact_By_Form(Partner.PartnerGUID, ContactModel);

                        if (result != null)
                        {
                            await DialogService.Alert($"{Localization["ContactRequest"]}", $"{Localization["Alert"]}");
                            DialogService.Close();
                        }
                        else
                        {
                            await DialogService.Alert($"{Localization["ContactError"]}", $"{Localization["Alert"]}");
                        }
                    }
                }
            }
            catch (Exception)
            {
                await DialogService.Alert($"{Localization["TryAgain"]}", $"{Localization["Alert"]}");
            }
        }

        public bool VerifyPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+(?:351|44|33|1|61|49|39|7|34|55|81|86|91|212|213|216|218)-\d{3}-\d{3}-\d{3}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }
        public bool VerifyEmail(string email)
        {
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
        private void Cancel()
        {
            DialogService.Close();
        }
    }
}
