using ContactManagement.MongoRepository;

namespace ContactManagement.ReportingSystem.Models;

// Talep edilen raporları içeren sınıf.

[BsonCollection("reportstatuses")]
public class ReportStatus : Document
{
    
    // Konum bilgisi.
    public string? LocationInfo { get; set; }
    
    // Raporun talep edildiği tarih.
    public DateTime? RequestDate { get; set; }

    // Raporun durumu.
    public string? Status { get; set; }
    
}