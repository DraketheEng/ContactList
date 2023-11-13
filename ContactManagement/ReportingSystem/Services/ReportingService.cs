using ContactList.ContactList.Reporting.Services;
using ContactManagement.Contact.Models;
using ContactManagement.MongoRepository;
using ContactManagement.ReportingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.ReportingSystem.Services
{
    public class ReportingService : ControllerBase, IReportingService
    {
        private readonly IMongoDbServices<ReportStatus> _reportStatusRepository;
        private readonly IMongoDbServices<Report> _detailedReportRepository;
        private readonly IMongoDbServices<Contacts> _contactRepository;
        private readonly global::ContactManagement.RabbitMqService.RabbitMqService _rabbitMqService;

        public ReportingService(
            IMongoDbServices<ReportStatus> reportStatusRepository,
            IMongoDbServices<Report> detailedReportRepository, IMongoDbServices<Contacts> contactRepository, global::ContactManagement.RabbitMqService.RabbitMqService rabbitMqService)
        {
            _reportStatusRepository = reportStatusRepository;
            _detailedReportRepository = detailedReportRepository;
            _contactRepository = contactRepository;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<bool> RequestReport(string location)
        {
            // Rapor talep edildiğinde yapılacak işlemler
            var reportId = Guid.NewGuid().ToString();
            var requestDate = DateTime.UtcNow;

            // Asenkron bir görev olarak rapor oluşturma işlemini başlat
            _ = GenerateReportAsync(reportId, location, requestDate);

            // Talep edilen raporun kimlik bilgisini kullanıcıya döndür
            return true;
        }

        public async Task<IEnumerable<ReportStatus>> GetReportStatuses()
        {
            // Tüm rapor durumlarını getir
            var reportStatuses = await _reportStatusRepository.GetAllAsync();
            return reportStatuses;
        }

        public async Task<Report> GetReportDetails(string reportId)
        {
            // Belirli bir raporun detaylarını getir
            var reportDetails = await _detailedReportRepository.GetById(reportId);
            return reportDetails;
        }

        public async Task GenerateReportAsync(string reportId, string location, DateTime requestDate)
        {
            // ReportStatus oluştur ve durumu "Hazırlanıyor" olarak işaretle
            var reportStatus = new ReportStatus
            {
                LocationInfo = location,
                RequestDate = DateTime.Now,
                Id = reportId.ToString(),
                CreateDate = requestDate,
                Status = "Hazırlanıyor",
            };

            await _reportStatusRepository.CreateAsync(reportStatus);

            // RabbitMQ servisini başlat
            await _rabbitMqService.StartAsync(CancellationToken.None);

            try
            {
                // Raporu oluşturmak için asenkron bir işlem başlat
                // Bu işlem sonucunda rehberdeki kayıtlı kişiler için istatistikleri çıkarabilirsiniz

                // Raporu tamamlandığında durumu "Tamamlandı" olarak güncelle
                
                var numberOfContacts = await GetNumberOfContactsAsync(location);
                var numberOfPhoneNumbers = await GetNumberOfPhoneNumbersAsync(location);

                var detailedReport = new Report()
                {
                    Id = reportId.ToString(),
                    CreateDate = requestDate,
                    LocationInfo = location,
                    NumberOfContacts = numberOfContacts,
                    NumberOfPhoneNumbers = numberOfPhoneNumbers,
                };

                await _detailedReportRepository.CreateAsync(detailedReport);

                // Raporun durumunu güncelle
                reportStatus.Status = "Tamamlandı";
                await _reportStatusRepository.ReplaceOneAsync(reportStatus);
            }
            finally
            {
                // RabbitMQ servisini durdur
                await _rabbitMqService.StopAsync(CancellationToken.None);
            }
        }
        
        private async Task<int> GetNumberOfContactsAsync(string location)
        {
            // Belli bir konumdaki kayıtlı kişi sayısını elde etmek için gerekli sorgu

            var contactList = await _contactRepository.FilterByAsync(c => c.CommunicationInfo.Any(ci => ci.City == location));

            var numberOfContacts = contactList.Count();

            return numberOfContacts;
        }


        private async Task<int> GetNumberOfPhoneNumbersAsync(string location)
        {
            // Belli bir konumdaki kayıtlı telefon numarası sayısını elde etmek için gerekli sorgu
            var contactList = await _contactRepository.FilterByAsync(c => c.CommunicationInfo.Any(ci => ci.City == location));

            // Her bir kişiye ait farklı telefon numaralarını toplamak için bir küme oluştur
            var uniquePhoneNumbers = new HashSet<string>();

            // Her bir kişinin telefon numaralarını kümeye ekle
            foreach (var contact in contactList)
            {
                foreach (var communicationInfo in contact.CommunicationInfo)
                {
                    // Eğer aynı telefon numarası daha önce eklenmemişse, kümeye ekle
                    if (!string.IsNullOrEmpty(communicationInfo.PhoneNo) && uniquePhoneNumbers.Add(communicationInfo.PhoneNo))
                    {
                        // Eğer telefon numarası boş değilse ve eklenmişse, sayacı artır
                    }
                }
            }

            var numberOfPhoneNumbers = uniquePhoneNumbers.Count;

            return numberOfPhoneNumbers;
        }


    }
}
