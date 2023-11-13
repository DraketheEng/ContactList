using ContactManagement.Contact.Models;
using ContactManagement.MongoRepository;
using ContactManagement.ReportingSystem.Models;
using MongoDB.Driver;

namespace ContactManagement.Contact.Services;

public class ContactService : MongoDbServices<Contacts>, IContactService
{
    private readonly IMongoDbServices<Contacts> _dbServices;

    public ContactService(IMongoDbSettings settings, IMongoCollection<Contacts> contactCollection,
        IMongoDbServices<Contacts> dbServices) : base(settings, contactCollection)
    {
        _dbServices = dbServices;
    }

    public async Task<bool>
        Create(Contacts contact) // Yeni bir kişi oluşturur ardından veritabanına ekler.
    {
        contact.Id = Guid.NewGuid().ToString();

        // Eğer iletişim bilgisi varsa, her bir iletişim bilgisine bir ID atar
        if (contact.CommunicationInfo != null)
        {
            foreach (var communicationInfo in contact.CommunicationInfo)
            {
                if (communicationInfo != null)
                {
                    communicationInfo.Id = Guid.NewGuid().ToString();
                }
            }
        }

        await _dbServices.CreateAsync(contact);

        return true;
    }

    public async Task<bool> Delete(string id) // Belirli bir kişiyi database'den siler.
    {
        await _dbServices.DeleteByIdAsync(id);
        return true;
    }

    public async Task<List<Contacts>> GetAllContacts() // Tüm kişilerin bilgilerini getirir.
    {
        var list = await _dbServices.GetAllAsync();
        return list;
    }

    public async Task<ContactDetails> GetContactDetails(string contactId)
    {
        // İlgili kişiyi ID'ye göre Database'den çeker
        var contact = await _dbServices.GetById(contactId);

        if (contact == null)
        {
            // Belirtilen ID'ye sahip kişi bulunadıysa
            return null;
        }

        // Kişi bulunduysa ayrıntılı bilgileri içeren bir nesne oluştur
        var contactDetails = new ContactDetails
        {
            ContactId = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Firm = contact.Firm,
            CommunicationInfo = contact.CommunicationInfo
        };

        return contactDetails;
    }

    public async Task<bool> AddCommunicationInfo(string contactId,
            CommunicationInfo communicationInfo) // Belirli bir kişiye iletişim bilgisi ekler.
    {

        // Kişiyi ID'sine göre bul
        var contact = await _dbServices.GetById(contactId);

        // Eğer kişi bulunamazsa işlem başarısız
        if (contact == null)
            return false;
        
        // İletişim bilgisine bir ID ataması yap
        communicationInfo.Id = Guid.NewGuid().ToString();

        // Eğer kişinin iletişim bilgisi yoksa yeni bir liste oluştur
        if (contact.CommunicationInfo == null)
            contact.CommunicationInfo = new List<CommunicationInfo?>();

        // Yeni iletişim bilgisini kişinin listesine ekle
        contact.CommunicationInfo.Add(communicationInfo);

        // Kişiyi güncelle
        await _dbServices.ReplaceOneAsync(contact);

        return true;
    }

    public async Task<bool>
        RemoveCommunicationInfo(string contactId,
            string communicationInfoId) // Belirli bir kişiden iletişim bilgisi kaldırır.
    {
        // Kişiyi ID'sine göre bul
        var contact = await _dbServices.GetById(contactId);

        // Eğer kişi bulunamazsa veya kişinin iletişim bilgisi yoksa işlem başarısız
        if (contact == null || contact.CommunicationInfo == null)
            return false;

        // Kaldırılacak iletişim bilgisini bul
        var infoToRemove = contact.CommunicationInfo.FirstOrDefault(info => info?.Id == communicationInfoId);

        // Eğer bulunursa, kişinin listesinden kaldır ve kişiyi güncelle
        if (infoToRemove != null)
        {
            contact.CommunicationInfo.Remove(infoToRemove);
            await _dbServices.ReplaceOneAsync(contact);
            return true;
        }

        return false;
    }

    public class ContactDetails
    {
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Firm { get; set; }
        public List<CommunicationInfo> CommunicationInfo { get; set; }
    }
}