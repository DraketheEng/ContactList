using ContactManagement.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactManagement.Contact.Models
{

    // Bir kişinin iletişim bilgilerini temsil eden sınıf.
    
    [BsonCollection("communication_info")]
    
    public class CommunicationInfo : Document
    {

        // Kişinin telefon numarası.
        public string PhoneNo { get; set; }
        
        // Kişinin e-posta adresi.
        public string Email { get; set; }
        
        // Kişinin yaşadığı şehir bilgisi.
        public string City { get; set; }
        
    }
}