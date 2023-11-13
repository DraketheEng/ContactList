using ContactManagement.MongoRepository;

namespace ContactManagement.ReportingSystem.Models;
    
    // Rehberle ilgili istatistikleri içeren rapor sınıfı.
   
[BsonCollection("report")]
    public class Report : Document
    {

        // Konum bilgisi.
        public string? LocationInfo { get; set; }
        
        // Konumda yer alan rehbere kayıtlı kişi sayısı.
        public int? NumberOfContacts { get; set; }
        
        // Konumda yer alan rehbere kayıtlı telefon numarası sayısı.
        public int? NumberOfPhoneNumbers { get; set; }
        
    }