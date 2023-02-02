using ContactApp.Api.Auth;
using ContactApp.Api.Controllers.Contact.ControllerService;
using ContactApp.Api.Controllers.Contact.Models;
using ContactApp.Api.Helpers;
using ContactApp.Models.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactApp.Api.Controllers.Contact
{
    [Authorize]
    [ApiController]
    [Route("api/v1/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactControllerService _service;

        public ContactController(IContactControllerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all personal contacts
        /// </summary>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("", Name = "GetAllContacts")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(typeof(ContactApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetAllContacts([FromQuery] ContactSearchObject searchObject)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            var contacts = await _service.GetAllContactsAsync(searchObject, userId);
            return Ok(new ContactApiResponse
            {
                Data = contacts
            });
        }

        /// <summary>
        /// Gets favourites
        /// </summary>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("favourites", Name = "GetFavourites")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(typeof(ContactApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetFavourites()
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            var contacts = await _service.GetFavouritesAsync(userId);
            return Ok(new ContactApiResponse
            {
                Data = contacts
            });
        }

        /// <summary>
        /// Gets contact details
        /// </summary>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{contactId}", Name = "GetContactDetails")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(typeof(ContactApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetContactDetails([FromRoute] Guid contactId)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            var contact = await _service.GetContactDetailsAsync(contactId, userId);
            return Ok(new ContactApiResponse
            {
                Data = new List<ContactApp.Models.Contact> { contact }
            });
        }

        /// <summary>
        /// Creates new personal contact
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "name": "Adin",
        ///         "phone": "062-333-222",
        ///         "email": "example@gmail.com",
        ///         "address": "Mostar"
        ///     }
        /// </remarks>
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("create", Name = "CreateContact")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateContact([FromBody] ContactDto model)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            await _service.CreateContactAsync(model, userId);
            return Ok(new ContactApiResponse
            {
                Message = "New contact successfully created",
            });
        }

        /// <summary>
        /// Edits existing personal contact
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///     {
        ///         "id": "FECAE289-65C3-4F55-41F5-08DB03D98EE7",
        ///         "name": "Edit",
        ///         "phone": "062-333-222",
        ///         "email": "example@gmail.com",
        ///         "address": "Mostar bb"
        ///     }
        /// </remarks>
        [Authorize(Policy = "UserPolicy")]
        [HttpPut("edit", Name = "EditContact")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(typeof(ContactApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ContactApp.Models.Contact>> EditContact([FromBody] ContactDto model)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            var contact = await _service.EditContactAsync(model, userId);
            return Ok(new ContactApiResponse
            {
                Message = "Contact successfully edited",
                Data = new List<ContactApp.Models.Contact> { contact }
            });
        }

        /// <summary>
        /// Deletes existing personal contact
        /// </summary>
        /// <param name="contactId"></param>
        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("delete/{contactId}", Name = "DeleteContact")]
        [HasClaim(ClaimTypes.NameIdentifier)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteContact([FromRoute] Guid contactId)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User, ClaimTypes.NameIdentifier);
            await _service.DeleteContactAsync(contactId, userId);
            return Ok(new ContactApiResponse
            {
                Message = "Contact successfully deleted",
            });
        }
    }
}