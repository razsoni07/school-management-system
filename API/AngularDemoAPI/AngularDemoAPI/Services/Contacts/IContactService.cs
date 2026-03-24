using AngularDemoAPI.Models.Domain;
using AngularDemoAPI.Models.ViewModels.Contact;

namespace AngularDemoAPI.Services.Contacts
{
    public interface IContactService
    {
        List<Contact> GetAll();
        Contact? GetById(int id);
        Contact Add(AddContactRequestDTO request);
        Contact? Update(int id, AddContactRequestDTO request);
        bool Delete(int id);
    }
}
