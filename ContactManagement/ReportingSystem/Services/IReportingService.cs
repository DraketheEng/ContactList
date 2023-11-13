using ContactManagement.ReportingSystem.Models;

namespace ContactList.ContactList.Reporting.Services;

public interface IReportingService
{
    Task<bool> RequestReport(string location);
    Task<IEnumerable<ReportStatus>> GetReportStatuses();
    Task<Report> GetReportDetails(string reportId);
    
}