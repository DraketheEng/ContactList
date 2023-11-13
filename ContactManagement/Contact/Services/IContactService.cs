using ContactManagement.Contact.Models;
using ContactManagement.MongoRepository;

namespace ContactManagement.Contact.Services;

public interface IContactService : IMongoDbServices<Contacts>
{
    
    Task<List<Contacts>> GetAllContacts();
    
    Task<ContactService.ContactDetails> GetContactDetails(string contactId);
    
    Task<bool> Create(Contacts item);

    Task<bool> Delete(string id);

    Task<bool> AddCommunicationInfo(string contactId, CommunicationInfo communicationInfo);

    Task<bool> RemoveCommunicationInfo(string contactId, string communicationInfoId);
}