using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.JSInterop;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using Polly;
using Radzen;
using MainHub.Internal.PeopleAndCulture.Common;
namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.Components
{
    [Authorize(Policy = "Supervisor")]
    public partial class DialogPartnerRequestComponent
    {
        [Inject]
        private DialogService DialogService { get; set; } = null!;
        [Inject]
        private GraphServiceClient GraphClient { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private PartnerAppRepository PartnerRepository { get; set; } = null!;
        [Inject]
        private MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;
        protected User _user = new();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var request = GraphClient?.Me.Request();
                var requestValidated = (request == null) ? null : _user = await request.GetAsync();
            }
            catch (Exception ex)
            {
                ConsentHandler?.HandleException(ex);
            }
        }
        private readonly Context? _editContext;

        public PartnerModel _partnerModelItem = new();
        private async Task HandleValidSubmit()
        {
            try
            {
                if (_partnerModelItem == null ||
                    (string.IsNullOrEmpty(_partnerModelItem.Name) &&
                     string.IsNullOrEmpty(_partnerModelItem.TaxNumber) &&
                     string.IsNullOrEmpty(_partnerModelItem.ServiceDescription) &&
                     string.IsNullOrEmpty(_partnerModelItem.Observation) &&
                     string.IsNullOrEmpty(_partnerModelItem.Address) &&
                     string.IsNullOrEmpty(_partnerModelItem.PostalCode) &&
                     string.IsNullOrEmpty(_partnerModelItem.Locality) &&
                     string.IsNullOrEmpty(_partnerModelItem.PhoneNumber) &&
                     string.IsNullOrEmpty(_partnerModelItem.Country)))
                {
                    await DialogService.Alert($"{Localization["PleaseInsertRequestPartner"]}", $"{Localization["Alert"]}");
                }
                else
                {
                    _partnerModelItem.CreatedBy = _user.Id;

                    List<string> missingFields = new List<string>();

                    if (string.IsNullOrEmpty(_partnerModelItem.Name))
                    {
                        missingFields.Add(Localization["EnterName"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.TaxNumber))
                    {
                        missingFields.Add(Localization["EnterTaxNumber"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.ServiceDescription))
                    {
                        missingFields.Add(Localization["EnterService"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.Observation))
                    {
                        missingFields.Add(Localization["EnterObservation"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.Address))
                    {
                        missingFields.Add(Localization["EnterAddress"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.PostalCode))
                    {
                        missingFields.Add(Localization["EnterPostalCode"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.Locality))
                    {
                        missingFields.Add(Localization["EnterLocality"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.Country))
                    {
                        missingFields.Add(Localization["EnterCountry"]);
                    }

                    if (string.IsNullOrEmpty(_partnerModelItem.PhoneNumber))
                    {
                        missingFields.Add(Localization["EnterPhoneNumber"]);
                    }
                    else if (!VerifyPhoneNumber(_partnerModelItem.PhoneNumber))
                    {
                        await DialogService.Alert($"{Localization["InvalidPhoneNumber"]}", $"{Localization["Alert"]}");
                        return;
                    }
                    else if (!VerifyTaxNumber(_partnerModelItem.TaxNumber))
                    {
                        await DialogService.Alert($"{Localization["InvalidTaxNumber"]}", $"{Localization["Alert"]}");
                        return;
                    }

                    if (missingFields.Count > 0)
                    {
                        string missingFieldsList = string.Join($" {Localization["And"]} ", missingFields);
                        await DialogService.Alert($"{missingFieldsList} {Localization["Missing"]}, {Localization["Please"]}", $"{Localization["Alert"]}");
                    }
                    else
                    {
                        var result = await PartnerRepository.App_Create_Partner_By_Form(_partnerModelItem);

                        if (result != null)
                        {
                            await DialogService.Alert($"{Localization["PartnerSuccess"]}", $"{Localization["Alert"]}");
                            NavigationManager.NavigateTo("ShowPartnerData", true);
                        }
                        else
                        {
                            await DialogService.Alert($"{Localization["PartnerError"]}", $"{Localization["Alert"]}");
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
        public bool VerifyTaxNumber(string tax)
        {
            string pattern = @"^^\d{9}$$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(tax);
        }
        private void Cancel()
        {
            DialogService.Close();
        }
        private void RedirectToDashboard()
        {
            NavigationManager.NavigateTo($"Dashboard/");

        }
        private void RedirectToPartners()
        {
            NavigationManager.NavigateTo($"requestPartner/");
        }
    }
}
