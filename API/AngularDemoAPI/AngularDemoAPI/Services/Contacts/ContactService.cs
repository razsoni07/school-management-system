using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Domain;
using AngularDemoAPI.Models.ViewModels.Contact;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly AngularDemoDbContext _context;

        public ContactService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public List<Contact> GetAll()
        {
            return _context.Contacts.AsNoTracking().ToList();
        }

        public Contact? GetById(int id)
        {
            return _context.Contacts.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }

        public Contact Add(AddContactRequestDTO request)
        {
            var contact = new Contact
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Favorite = request.Favorite
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public Contact? Update(int id, AddContactRequestDTO request)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
                return null;

            contact.Name = request.Name;
            contact.Email = request.Email;
            contact.Phone = request.Phone;
            contact.Favorite = request.Favorite;

            _context.SaveChanges();

            return contact;
        }

        public bool Delete(int id)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
                return false;

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return true;
        }
    }
}
