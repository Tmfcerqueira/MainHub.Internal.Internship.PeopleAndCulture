using Microsoft.AspNetCore.Mvc;
using PartnerManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners.PCreate;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Miscellaneous;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.ContactExtensions.PContactCreate;
using PartnerManagement.Api.Models.API_Partners;
using PartnerManagement.Api.Models.API_Contact;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using PartnerManagement.DataBase.Models;
using YamlDotNet.Core;
using PartnerManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners;
using Azure;
using MainHub.Internal.PeopleAndCulture.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Controllers
{
    [Authorize(Policy = "Supervisor")]
    [Route("api/partner")]
    [Produces("application/json")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        public readonly IPartnerRepository PartnerRepository;
        public PartnerController(IPartnerRepository partnerRepository)
        {
            PartnerRepository = partnerRepository;
        }


        //**************************************************
        //*                POST api/partner                *
        //**************************************************


        /// <summary>
        /// Creates a Partner.
        /// </summary>
        /// <param name="partnerMapped">The requestModel Mapping</param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Partner
        ///     {
        ///         "name": "string",
        ///         "phoneNumber": "string",
        ///         "address": "string",
        ///         "postalCode": "string",
        ///         "country": "string",
        ///         "taxNumber": "string",
        ///         "serviceDescription": "string",
        ///         "observation": "string",
        ///         "CreatedBy": "string",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns if its ok</response>
        /// <response code="400">If the item is null</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiPartnerCreateResponseModel>> Post_Create_Partner(ApiPartnerCreateRequestModel partnerMapped)
        {
            if (partnerMapped == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            // converter API_REQUEST_MODEL em _PARTNER_REPO_MODEL
            var repoModel = partnerMapped.ToPartnerRepoModel();

            // chamar repository.createpartner com o PARTNER_REPOMODEL
            var createResponse = await PartnerRepository.Create_Partner_Async(repoModel);

            // na resposta do repositorio transformar o PARTNER_REPO_MODEL em API_RESPONSE_MODEL
            var response = createResponse.ToPartnerCreateResponseModel();
            // devolver OK com o objeto API_RESPONSE_MODEL
            return Ok(response);
        }


        //**************************************************
        //*                POST api/contacts               *
        //**************************************************


        /// <summary>
        /// Creates a Partner Contact.
        /// </summary>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Contact
        ///     {
        ///         "name": "string",
        ///         "email": "string",
        ///         "role": "string",
        ///         "phoneNumber": "string",
        ///         "department": "string",
        ///         "observation": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="partnerGuid">The contact ID</param>
        /// <param name="contactMapped">The requestModel Mapping</param>
        /// <response code="200">Returns if its ok</response>
        /// <response code="400">If the item is null</response>
        // POST api/Contact
        [HttpPost("{partnerGuid}/contact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiContactCreateResponseModel>> Post_Create_Contact(Guid partnerGuid, ApiContactCreateRequestModel contactMapped)
        {
            if (contactMapped == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            // converter API_REQUEST_MODEL em _PARTNER_REPO_MODEL
            var repoModel = contactMapped.ToContactRepoModel();
            repoModel.PartnerGUID = partnerGuid;

            // chamar repository.createpartner com o PARTNER_REPOMODEL
            var createResponse = await PartnerRepository.Create_Contact_Async(repoModel);

            // na resposta do repositorio transformar o PARTNER_REPO_MODEL em API_RESPONSE_MODEL
            var response = createResponse.ToContactCreateResponseModel();

            // devolver OK com o objeto API_RESPONSE_MODEL 
            return Ok(response);
        }


        //**************************************************
        //*                GETS Partner                    *
        //**************************************************

        /// <summary>
        /// Get a contact for a specific partnerguid.
        /// </summary>
        /// <param name="partnerGuid">The contact ID</param>
        /// <returns>IEnumerable of type ApiPartnerResponseModel with a specific contact partnerGuid</returns>
        /// <response code="200">Returns the IEnumerable of ApiPartnerResponseModel</response>
        /// <response code="404">If no Partners are found for the partnerId</response>
        [HttpGet("{partnerGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiPartnerResponseModel>> Get_Partner_By_Guid_Async(Guid partnerGuid)
        {
            var partner = await PartnerRepository.Get_Partner_By_Guid_Async(partnerGuid);

            if (partner == null)
            {
                return NotFound("Partner not found");
            }
            return Ok(partner.ToPartnerResponseModel());
        }

        /// <summary>
        /// Get all partners
        /// </summary>
        /// <param name="num_page">The Pagination num page</param>
        /// <param name="pageSize">The Pagination pagesize</param>
        /// <param name="name">The name to make the search</param>
        /// <returns>IEnumerable of ApiPartnerResponseModel objects</returns>
        /// <response code="200">Returns the IEnumerable of ApiPartnerResponseModel objects</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ApiPartnerResponseWithCount>> Get_All_Partners_Async(int num_page, int pageSize, string? name)
        {
            try
            {
                if (num_page < 1)
                {
                    num_page = 1;
                }

                if (pageSize < 1 || pageSize > 20)
                {
                    pageSize = 20;
                }

                var partner = await PartnerRepository.Get_All_Partners_Async(num_page, pageSize, name);

                if (partner.Item1 == null || !partner.Item1.Any())
                {
                    return NotFound();
                }

                var partnerResponse = partner.Item1.Select(a => a.ToPartnerResponseModel()).ToList();

                var partnerPagingModel = new ApiPartnerResponseWithCount()
                {
                    Partners = partnerResponse,
                    TotalCount = partner.count,
                };

                return Ok(partnerPagingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //**************************************************
        //*                GETS Contact                    *
        //**************************************************


        /// <summary>
        ///  Get's each contact of a specific partnerGuid
        /// </summary>
        /// <param name="partnerGuid">The Partner ID</param>
        /// <param name="num_page">The Pagination num page</param>
        /// <param name="pageSize">The Pagination pagesize</param>
        /// <param name="name">The name to make the search</param>
        /// <returns>An ApiContactResponseModel which has each contact of a specific contact, or null if the contact does not exist</returns>
        /// <response code="200">Returns each Contact of a specific contact</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpGet("{partnerGuid}/contact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiContactResponseWithCount>> Get_All_Contacts_From_One_Partner(Guid partnerGuid, int num_page, int pageSize, string? name)
        {
            try
            {
                if (num_page < 1)
                {
                    num_page = 1;
                }

                if (pageSize < 1 || pageSize > 20)
                {
                    pageSize = 20;
                }

                var contact = await PartnerRepository.Get_Partner_Contacts_Async(partnerGuid, num_page, pageSize, name);

                if (contact.Item1 == null || !contact.Item1.Any())
                {
                    return NotFound();
                }

                var contactResponse = contact.Item1.Select(a => a.ToContactResponseModel()).ToList();

                var contactPagingModel = new ApiContactResponseWithCount()
                {
                    Contacts = contactResponse,
                    TotalCount = contact.count,
                };

                return Ok(contactPagingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        ///  Get's a specific Contact of a specific Partner.
        /// </summary>
        /// <param name="partnerGuid">The Partner ID</param>
        /// <param name="contactGuid">The Contact ID</param>
        /// <returns>An PartnerRepoModel object for a specific Contact and Partner, or null if the Contact or Partner doesn't exist</returns>
        /// <response code="200">Returns the PartnerRepoModel object for a specific Contact and Partner</response>
        /// <response code="404">If the Contact or Partner doesn't exist</response>
        [HttpGet("{partnerGuid}/contact/{contactGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiContactResponseModel>> Get_Specific_Contact_From_One_Partner(Guid partnerGuid, Guid contactGuid)
        {
            var partner = await PartnerRepository.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid);
            if (partner == null)
            {
                return NotFound("Contact or Partner doesn't exist");
            }

            return Ok(partner.ToContactResponseModel());
        }


        //**************************************************
        //*               Update Partner                   *
        //**************************************************

        /// <summary>
        /// Update an existing contact.
        /// </summary>
        /// <param name="partnerGuid">Parameter that will let me find from who this Contact belongs</param>
        /// <param name="partnerUpdateModel">Model that manages the whole update</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the contact is updated successfully</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpPut("{partnerGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update_Partner(Guid partnerGuid, [FromBody] PartnerUpdateModel partnerUpdateModel)
        {
            var partner = await PartnerRepository.Get_Partner_By_Guid_Async(partnerGuid);

            if (partner == null)
            {
                return NotFound();
            }

            var repoModel = partnerUpdateModel.ToPartnerRepoModel();

            await PartnerRepository.Update_Partner_Async(repoModel);

            return NoContent();
        }


        //**************************************************
        //*               Update Contact                   *
        //**************************************************

        /// <summary>
        /// Update an existing Contact.
        /// </summary>
        /// <param name="contactUpdateModel">Model that manages the whole update</param>
        /// <param name="partnerGuid">Parameter that will let me find from who this Contact belongs</param>
        /// <param name="contactGuid">Parameter that will let me find from who this Contact belongs</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the Contact is updated successfully</response>
        /// <response code="404">If the Contact does not exist</response>
        [HttpPut("{partnerGuid}/contact/{contactGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update_Contact(Guid partnerGuid, Guid contactGuid, [FromBody] ContactUpdateModel contactUpdateModel)
        {
            var contact = await PartnerRepository.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid);

            if (contact == null)
            {
                return NotFound();
            }

            var repoModel = contactUpdateModel.ToContactRepoModel();

            await PartnerRepository.Update_Contact_Async(repoModel);

            return NoContent();
        }



        //**************************************************
        //*               Delete Partner                   *
        //**************************************************

        /// <summary>
        /// Deletes an Partner.
        /// </summary>
        /// <param name="partnerGuid">The contact ID</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the contact is deleted successfully</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpDelete("{partnerGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete_Partner(Guid partnerGuid)
        {
            var partner = await PartnerRepository.Delete_Partner_Async(partnerGuid);

            if (!partner)
            {
                return NotFound();
            }
            return NoContent();
        }


        //**************************************************
        //*               Delete Contact                   *
        //**************************************************

        /// <summary>
        /// Deletes an Contact.
        /// </summary>
        /// <param name="partnerGuid">The partner ID</param>
        /// <param name="contactGuid">THe contact id</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the contact is deleted successfully</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpDelete("{partnerGuid}/contact/{contactGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete_Contact(Guid partnerGuid, Guid contactGuid)
        {
            var contact = await PartnerRepository.Delete_Contact_Async(partnerGuid, contactGuid);

            if (!contact)
            {
                return NotFound();
            }
            return NoContent();
        }


        /// <summary>
        /// Deletes every Contact.
        /// </summary>
        /// <param name="partnerGuid">The partner ID</param>
        /// <returns></returns>
        /// <response code="204">Returns no content if the contact is deleted successfully</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpDelete("{partnerGuid}/contact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete_All_Contacts(Guid partnerGuid)
        {
            var delete = await PartnerRepository.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid);

            if (!delete)
            {
                NotFound();
            }

            return NoContent();
        }

        //**************************************************
        //*                  History Gets                  *
        //**************************************************

        ///<summary>
        ///Gets every Partner History with specific Action
        /// </summary>
        /// <param name="partnerGUID">PartnerGUid for routing</param>
        /// <param name="num_page">The Pagination num page</param>
        /// <param name="pageSize">The Pagination pagesize</param>
        /// <returns></returns>
        /// <response code="204">Returns Model list with if the history is different of null or empty</response>
        /// <response code="404">If the action is wrong or model does not exist</response>
        [HttpGet("{partnerGUID}/partnerhistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiPartnerHistoryResponseWithCount>> Get_All_Partners_History_Async(Guid partnerGUID, int num_page, int pageSize)
        {
            if (num_page < 1)
            {
                num_page = 1;
            }

            if (pageSize < 1 || pageSize > 20)
            {
                pageSize = 20;
            }

            var partnerHistory = await PartnerRepository.Get_All_Partners_History_Async(partnerGUID, num_page, pageSize);

            if (partnerHistory.Item1 == null || !partnerHistory.Item1.Any())
            {
                return NotFound();
            }

            var partnerHistoryResponse = partnerHistory.Item1.Select(a => a.ToPartnerHistoryResponseModel()).ToList();

            var partnerPagingModel = new ApiPartnerHistoryResponseWithCount()
            {
                Partners = partnerHistoryResponse,
                TotalCount = partnerHistory.count,
            };

            return Ok(partnerPagingModel);
        }

        ///<summary>
        ///Gets every Contact History with specific Action
        /// </summary>
        /// <param name="partnerGUID">The partnerGUID to get all contacts belonged to him</param>
        /// <param name="contactGuid">The contactGuid to get specific contact belonged to him</param>
        /// <param name="num_page">The Pagination num page</param>
        /// <param name="pageSize">The Pagination pagesize</param>
        /// <returns></returns>
        /// <response code="204">Returns Model list with if the history is different of null or empty</response>
        /// <response code="404">If the contact does not exist</response>
        [HttpGet("{partnerGUID}/contact/{contactGuid}/contacthistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiContactHistoryResponseWithCount>> Get_All_Contacts_History_Async(Guid partnerGUID, Guid contactGuid, int num_page, int pageSize)
        {
            if (num_page < 1)
            {
                num_page = 1;
            }

            if (pageSize < 1 || pageSize > 20)
            {
                pageSize = 20;
            }

            var contactHistory = await PartnerRepository.Get_All_Contacts_History_Async(partnerGUID, contactGuid, num_page, pageSize);

            if (contactHistory.Item1 == null || !contactHistory.Item1.Any())
            {
                return NotFound();
            }

            var contactHistoryResponse = contactHistory.Item1.Select(a => a.ToContactHistoryResponseModel()).ToList();

            var contactPagingModel = new ApiContactHistoryResponseWithCount()
            {
                Contacts = contactHistoryResponse,
                TotalCount = contactHistory.count,
            };

            return Ok(contactPagingModel);
        }

        //**************************************************
        //*                    METODOS                     *
        //**************************************************
    }
}
