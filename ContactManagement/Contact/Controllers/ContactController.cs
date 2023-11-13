using ContactManagement.Contact.Models;
using ContactManagement.Contact.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactManagement.Contact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        
        // HTTP GET ile tüm kişilerin listesini getirir.
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Contacts>>> GetAllContacts()
        {
            var result = await _contactService.GetAllContacts();
            return Ok(result);
        }
        
        // HTTP GET ile belirli bir kişinin ayrıntılı bilgilerini getirir.
        [HttpGet("[action]")]
        public async Task<ActionResult<ContactService.ContactDetails>> GetContactDetails(string contactId)
        {
            var contactDetails = await _contactService.GetContactDetails(contactId);

            if (contactDetails == null)
            {
                return NotFound();
            }

            return Ok(contactDetails);
        }

        // HTTP POST ile yeni bir kişi oluşturur.
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> Create([FromBody] Contacts item)
        {
            var result = await _contactService.Create(item);
            return result ? Ok(true) : BadRequest("Failed to create contact.");
        }

        // HTTP DELETE ile belirli bir kişiyi siler.
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _contactService.Delete(id);
            return result ? Ok(true) : BadRequest("Failed to delete contact.");
        }

        // HTTP POST ile belirli bir kişiye iletişim bilgisi ekler.
        [HttpPost("AddCommunicationInfo/{contactId}")]
        public async Task<ActionResult<bool>> AddCommunicationInfo(string contactId, [FromBody] CommunicationInfo communicationInfo)
        {
            var result = await _contactService.AddCommunicationInfo(contactId, communicationInfo);
            return result ? Ok(true) : BadRequest("Failed to add communication info.");
        }

        // HTTP DELETE ile belirli bir kişiden iletişim bilgisi kaldırır.
        [HttpDelete("RemoveCommunicationInfo/{contactId}/{communicationInfoId}")]
        public async Task<ActionResult<bool>> RemoveCommunicationInfo(string contactId, string communicationInfoId)
        {
            var result = await _contactService.RemoveCommunicationInfo(contactId, communicationInfoId);
            return result ? Ok(true) : BadRequest("Failed to remove communication info.");
        }
    }
}
