using AngularDemoAPI.Models.ViewModels.Contact;
using AngularDemoAPI.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [Route("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var contacts = _contactService.GetAll();
            return Ok(contacts);
        }

        [HttpGet]
        [Route("GetContactById/{id}")]
        public IActionResult GetContactById(int Id)
        {
            var contact = _contactService.GetById(Id);

            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult AddContact(AddContactRequestDTO request)
        {
            var contact = _contactService.Add(request);
            return Ok(contact);
        }

        [HttpPut]
        [Route("UpdateContact/{id}")]
        public IActionResult UpdateContact(int id, AddContactRequestDTO request)
        {
            var contact = _contactService.Update(id, request);

            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteContact(int id)
        {
            _contactService.Delete(id);
            return Ok();
        }
    }
}
