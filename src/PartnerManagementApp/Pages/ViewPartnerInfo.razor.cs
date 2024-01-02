using System;
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
    public partial class ViewPartnerInfo
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
        protected PartnerModel _partnerModel = new();
        private int Picker { get; set; }
        private (List<ContactModel> contacts, int count) _contactModelData = (new List<ContactModel>(), 0);

        private readonly List<string> _status = new List<string>
        {
            "Active",
            "Inactive",
        };
        protected override async void OnParametersSet()
        {
            try
            {
                _partnerModel = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);
                if (_partnerModel == null)
                {
                    await DialogService.Alert($"{Localization["NoPartnerInfo"]}", $"{Localization["NoInfo"]}", new AlertOptions() { OkButtonText = "Ok" });
                    NavigationManager.NavigateTo("ShowPartnerData/");
                }
            }
            catch (Exception ex)
            {
                await DialogService.Alert($"{Localization["NoPartnerInfo"]}", $"{Localization["NoInfo"]}", new AlertOptions() { OkButtonText = "Ok" });
                Console.WriteLine(ex);
                NavigationManager.NavigateTo("ShowPartnerData/");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _partnerModel = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);
            }
            catch (Exception ex)
            {
                await DialogService.Alert($"{Localization["NoPartnerInfo"]}", $"{Localization["NoInfo"]}", new AlertOptions() { OkButtonText = "Ok" });
                Console.WriteLine(ex);
                NavigationManager.NavigateTo("ShowPartnerData/");
            }
        }
        private async Task ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
            var a = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);

            _partnerModel = a;
        }

        public async Task DeletePartner(Guid partnerGuid)
        {
            try
            {
                _contactModelData = await PartnerRepository.Get_All_Contacts_Async(PGuid, 1, 5, string.Empty);

                var confirmationMessage = _contactModelData.count != 0 ? $"{Localization["EraseAllDataPartner"]}" : $"{Localization["ErasePartner"]}";
                var confirmation = await DialogService.Confirm(confirmationMessage, $"{Localization["PartnerInformation"]}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });

                if (confirmation == true)
                {
                    bool model;

                    if (_contactModelData.count >= 1)
                    {
                        model = await PartnerRepository.Delete_All_Contacts_Async(partnerGuid);
                        if (model)
                        {
                            model = await PartnerRepository.Delete_Partner_Async(partnerGuid);
                        }
                        else
                        {
                            await DialogService.Alert($"{Localization["ErrorDeleteContact"]}", $"{Localization["Alert"]}");
                        }
                    }
                    else
                    {
                        model = await PartnerRepository.Delete_Partner_Async(partnerGuid);
                    }

                    if (model)
                    {
                        await DialogService.Alert($"{Localization["ApproveDeleteContact"]}", $"{Localization["Alert"]}");
                        NavigationManager.NavigateTo($"ShowPartnerData/");
                    }
                    else
                    {
                        await DialogService.Alert($"{Localization["ErrorDeleteContact"]}", $"{Localization["Alert"]}");
                    }
                }
                else
                {
                    DialogService.Close();
                }

            }
            catch (ApiException apiex)
            {
                if (apiex.ErrorCode == 404)
                {
                    var confirmationMessage = $"{Localization["ErasePartner"]}";
                    var confirmation = await DialogService.Confirm(confirmationMessage, $"{Localization["PartnerInformation"]}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });

                    if (confirmation == true)
                    {
                        bool model;

                        model = await PartnerRepository.Delete_Partner_Async(partnerGuid);
                        if (model)
                        {
                            await DialogService.Alert($"{Localization["ApproveDeleteContact"]}", $"{Localization["Alert"]}");
                            NavigationManager.NavigateTo($"ShowPartnerData/");
                        }
                        else
                        {
                            await DialogService.Alert($"{Localization["ErrorDeleteContact"]}", $"{Localization["Alert"]}");
                        }
                    }
                }
                StateHasChanged();
            }
        }
        private async void SaveChanges()
        {
            try
            {
                if (_partnerModel == null ||
                   (string.IsNullOrEmpty(_partnerModel.Name) &&
                    string.IsNullOrEmpty(_partnerModel.TaxNumber) &&
                    string.IsNullOrEmpty(_partnerModel.ServiceDescription) &&
                    string.IsNullOrEmpty(_partnerModel.Observation) &&
                    string.IsNullOrEmpty(_partnerModel.Address) &&
                    string.IsNullOrEmpty(_partnerModel.PostalCode) &&
                    string.IsNullOrEmpty(_partnerModel.Locality) &&
                    string.IsNullOrEmpty(_partnerModel.PhoneNumber) &&
                    string.IsNullOrEmpty(_partnerModel.Country)))
                {
                    await DialogService.Alert($"{Localization["PleaseInsert"]}", $"{Localization["Alert"]}");
                }
                else
                {
                    List<string> missingFields = new List<string>();

                    if (string.IsNullOrEmpty(_partnerModel.Name))
                    {
                        missingFields.Add(Localization["EnterName"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.TaxNumber))
                    {
                        missingFields.Add(Localization["EnterTaxNumber"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.ServiceDescription))
                    {
                        missingFields.Add(Localization["EnterService"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.Observation))
                    {
                        missingFields.Add(Localization["EnterObservation"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.Address))
                    {
                        missingFields.Add(Localization["EnterAddress"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.PostalCode))
                    {
                        missingFields.Add(Localization["EnterPostalCode"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.Locality))
                    {
                        missingFields.Add(Localization["EnterLocality"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.Country))
                    {
                        missingFields.Add(Localization["EnterCountry"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModel.PhoneNumber))
                    {
                        missingFields.Add(Localization["EnterPhoneNumber"]);
                    }

                    if (missingFields.Count > 0)
                    {
                        string missingFieldsList = string.Join($" {Localization["And"]} ", missingFields);
                        await DialogService.Alert($"{missingFieldsList} {Localization["Missing"]}, {Localization["Please"]}", $"{Localization["Alert"]}");
                    }
                    else if (!VerifyPhoneNumber(_partnerModel.PhoneNumber))
                    {
                        await DialogService.Alert($"{Localization["InvalidPhoneNumber"]}", $"{Localization["Alert"]}");
                    }
                    else if (!VerifyTaxNumber(_partnerModel.TaxNumber))
                    {
                        await DialogService.Alert($"{Localization["InvalidTaxNumber"]}", $"{Localization["Alert"]}");
                    }
                    else
                    {
                        var a = await PartnerRepository.Update_Partner_Async(PGuid, _partnerModel);
                        if (a)
                        {
                            await DialogService.Alert($"{Localization["PartnerEditted"]}", $"{Localization["Alert"]}");

                            ToggleEditMode();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/DialogCardPage/{PGuid}");
                        }
                        StateHasChanged();
                    }
                }
            }
            catch (ApiException)
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

        private void RedirectToPartnerMenu()
        {
            NavigationManager.NavigateTo($"ViewPartnerInfo/{PGuid}");
        }
        private async Task RedirectContactList(Guid partnerGuid)
        {
            NavigationManager.NavigateTo($"/DialogCardPage/{partnerGuid}");

            StateHasChanged();
        }

        private async Task RedirectRequest()
        {
            PartnerModel model = await PartnerRepository.Get_Partner_By_Guid_Async(PGuid);

            await DialogService.OpenAsync<DialogRequestContactComponent>($"{Localization["ContactCreation"]}", new Dictionary<string, object>() { { "Partner", model } }, new DialogOptions()
            {
                Width = "1260px",
                Height = "430px",
                ShowTitle = true,
                CloseDialogOnOverlayClick = true,
                Style = "margin-top:4%;overflow:hidden !important;",
            });
        }
        void ShowTooltip1(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["SaveTtp"]}", options);
        void ShowTooltip2(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["BackTtp"]}", options);
        void ShowTooltip3(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["CloseTtp"]}", options);
        void ShowTooltip4(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["HistoryTtp"]}", options);
        void ShowTooltip5(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["DeleteTtp"]}", options);
        void ShowTooltip6(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["EditTtp"]}", options);
        void ShowTooltip7(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["ViewContactTtp"]}", options);
        void ShowTooltip8(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["ContactRedirectTtp"]}", options);
        void ShowTooltip10(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, $"{Localization["HelpTtp"]}", options);


        private async Task ShowDataHistory(int formPicker, Guid partnerGuid)
        {
            PartnerModel model = await PartnerRepository.Get_Partner_By_Guid_Async(partnerGuid);

            await DialogService.OpenAsync<DialogShowHistoryComponent>($"{Localization["PartnerHistory"]}", new Dictionary<string, object>() { { "Partner", model }, { "Picker", formPicker } }, new DialogOptions()
            {
                Width = "1100px",
            });
        }
        private async void GoBack()
        {
            NavigationManager.NavigateTo($"ShowPartnerData/");
        }
    }
}
