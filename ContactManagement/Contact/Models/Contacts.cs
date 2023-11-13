using System.Collections.Generic;
using ContactManagement.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactManagement.Contact.Models
{
    
    // Rehberdeki bir kişiyi temsil eden sınıf.

    [BsonCollection("contacts")]
    public class Contacts : Document
    {
        
        // Kişinin adı.
        public string Name { get; set; }
        
        // Kişinin soyadı.
        public string Surname { get; set; }
        
        // Kişinin çalıştığı firma.
        public string Firm { get; set; }
        
        // Kişinin iletişim bilgilerini içeren liste. 
        // Bir kişiye birden fazla iletişim bilgisi eklenebilir.
        public List<CommunicationInfo>? CommunicationInfo { get; set; }
        
    }
}