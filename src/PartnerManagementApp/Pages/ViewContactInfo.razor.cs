using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Polly;
using Radzen;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Pages
{
    [Authorize(Policy = "Supervisor")]
    public partial class ViewContactInfo
    {
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;

        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private TooltipService TooltipService { get; set; } = null!;
        [Parameter]
        public bool IsEditMode { get; set; }
        [Parameter]
        public Guid PGuid { get; set; }
        [Parameter]
        public Guid CGuid { get; set; }
        protected ContactModel _contactModel = new();
        protected PartnerModel _partnerModel = new();
        private int Picker { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _contactModel = await PartnerRepository.Get_Specific_Contact_From_One_Partner(PGuid, CGuid);
                _partnerModel = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);
            }
            catch (Exception ex)
            {
                await DialogService.Alert($"{Localization["NoPartnerInfo"]}", $"{Localization["NoInfo"]}", new AlertOptions() { OkButtonText = "Ok" });
                Console.WriteLine(ex);
                NavigationManager.NavigateTo("ShowPartnerData/");
            }
        }
        private async Task SaveChanges()
        {
            try
            {
                if (_contactModel == null ||
                    (string.IsNullOrEmpty(_contactModel.Name) &&
                     string.IsNullOrEmpty(_contactModel.Email) &&
                     string.IsNullOrEmpty(_contactModel.Role) &&
                     string.IsNullOrEmpty(_contactModel.PhoneNumber) &&
                     string.IsNullOrEmpty(_contactModel.Department) &&
                     string.IsNullOrEmpty(_contactModel.Observation)))
                {
                    await DialogService.Alert($"{Localization["InsertContact"]}", $"{Localization["Alert"]}");
                }
                else
                {
                    List<string> missingFields = new List<string>();

                    if (string.IsNullOrEmpty(_contactModel.Name))
                    {
                        missingFields.Add(Localization["EnterName"]);
                    }

                    if (string.IsNullOrEmpty(_contactModel.Email))
                    {
                        missingFields.Add(Localization["EnterEmail"]);
                    }

                    if (string.IsNullOrEmpty(_contactModel.Role))
                    {
                        missingFields.Add(Localization["EnterRole"]);
                    }

                    if (string.IsNullOrEmpty(_contactModel.PhoneNumber))
                    {
                        missingFields.Add(Localization["EnterPhoneNumber"]);
                    }

                    if (string.IsNullOrEmpty(_contactModel.Department))
                    {
                        missingFields.Add(Localization["EnterDepartment"]);
                    }

                    if (string.IsNullOrEmpty(_contactModel.Observation))
                    {
                        missingFields.Add(Localization["EnterObservation"]);
                    }

                    if (missingFields.Count > 1)
                    {
                        string missingFieldsList = string.Join($" {Localization["And"]} ", missingFields);
                        await DialogService.Alert($"{missingFieldsList} {Localization["Missing"]}, {Localization["Please"]}", $"{Localization["Alert"]}");
                    }
                    else if (missingFields.Count == 0)
                    {
                        if (!VerifyPhoneNumber(_contactModel.PhoneNumber))
                        {
                            await DialogService.Alert($"{Localization["InvalidPhoneNum"]}", $"{Localization["Alert"]}");
                        }
                        else if (!VerifyEmail(_contactModel.Email))
                        {
                            await DialogService.Alert($"{Localization["InvalidEmail"]}", $"{Localization["Alert"]}");
                        }
                        else
                        {
                            var model = await PartnerRepository.Update_Contact_Async(PGuid, CGuid, _contactModel);
                            if (model)
                            {
                                await DialogService.Alert($"{Localization["ContactApproved"]}", $"{Localization["Alert"]}");
                                ToggleEditMode();
                            }
                            else
                            {
                                await DialogService.Alert($"{Localization["ContactNotApproved"]}", $"{Localization["Alert"]}");
                            }
                            DialogService.Close();
                        }
                    }
                    else
                    {
                        await DialogService.Alert($"{Localization["Enter"]} {missingFields[0]}", $"{Localization["Alert"]}");
                    }
                }
            }
            catch (ApiException)
            {
                await DialogService.Alert($"{Localization["TryAgain"]}", $"{Localization["Alert"]}");
            }
            StateHasChanged();
        }



        private async void DeleteContact()
        {
            try
            {
                var confirmation = await DialogService.Confirm($"{Localization["ContactDecide"]}", $"{Localization["ContactDelete"]}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });

                if (confirmation == true)
                {
                    var model = await PartnerRepository.Delete_Contact_Async(PGuid, CGuid);

                    await DialogService.Alert($"{Localization["ApproveDeleteContact"]}", $"{Localization["Alert"]}");
                }
                else
                {
                    DialogService.Close();
                }
            }
            catch (ApiException)
            {
                await DialogService.Alert($"{Localization["TryAgain"]}", $"{Localization["Alert"]}");
            }
            StateHasChanged();
        }
        private async Task ToggleEditMode()
        {
            IsEditMode = !IsEditMode;

            var a = await PartnerRepository.Get_Specific_Contact_From_One_Partner(PGuid, CGuid);

            _contactModel = a;
        }
        private async void CancelEdit()
        {
            _contactModel = await PartnerRepository.Get_Specific_Contact_From_One_Partner(PGuid, CGuid);
        }
        private async void GoBack()
        {
            NavigationManager.NavigateTo($"DialogCardPage/{PGuid}/");
        }
        void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Shows a form to Create contact on clicking it!", options);
        void ShowTooltip1(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Clock button shows the History!, User plus shows a form to create a Contact, Some buttons to Edit and Delete any contact!", options);
        void ShowTooltip4(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "History", options);
        void ShowTooltip0(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Saves the data written in the form", options);
        void ShowTooltip2(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Redirects to the previous page", options);
        void ShowTooltip3(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Closes the menu", options);
        void ShowTooltip9(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "View history table", options);
        void ShowTooltip5(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Erases all data belonged to this Partner and himself", options);
        void ShowTooltip6(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Toggles edit mode", options);
        void ShowTooltip7(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "View every Contact from this Partner", options);
        void ShowTooltip8(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Redirects to the create Contact form", options);
        private async Task ShowDataHistory(int formPicker, Guid partnerGuid)
        {
            PartnerModel model = await PartnerRepository.Get_Partner_By_Guid_Async(partnerGuid);

            ContactModel model2 = await PartnerRepository.Get_Specific_Contact_From_One_Partner(partnerGuid, CGuid);

            if (model != null && model2 != null)
            {
                await DialogService.OpenAsync<DialogShowHistoryComponent>($"{Localization["ContactHistory"]}", new Dictionary<string, object>() { { "Contact", model2 }, { "Picker", formPicker }, { "Partner", model } }, new DialogOptions()
                {
                    Width = "1100px",
                });
                StateHasChanged();
            }
        }

        private async Task RedirectRequest()
        {
            PartnerModel model = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);

            await DialogService.OpenAsync<DialogRequestContactComponent>($"{Localization["CreateContact"]}", new Dictionary<string, object>() { { "Partner", model } }, new DialogOptions()
            {
                Width = "1260px",
                Height = "430px",
                ShowTitle = true,
                CloseDialogOnOverlayClick = true,
                Style = "margin-top:4%;overflow:hidden !important;",
            });

            StateHasChanged();
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
        public bool VerifyTaxNumber(string tax)
        {
            string pattern = @"^^\d{9}$$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(tax);
        }
        private void RedirectToPartners()
        {
            NavigationManager.NavigateTo($"ShowPartnerData/");
        }
        private void RedirectToContacts()
        {
            NavigationManager.NavigateTo($"DialogCardPage/{PGuid}/");
        }

        private void RedirectToContactMenu()
        {
            NavigationManager.NavigateTo($"ViewContactInfo/{PGuid}/{CGuid}");
        }
        private void RedirectToPartnerMenu()
        {
            NavigationManager.NavigateTo($"ViewPartnerInfo/{PGuid}");
        }
    }
}
