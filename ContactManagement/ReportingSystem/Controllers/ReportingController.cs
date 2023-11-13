using ContactList.ContactList.Reporting.Services;
using ContactManagement.ReportingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.ReportingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Guid>> RequestReport([FromBody] string location)
        {
            try
            {
                var reportId = await _reportingService.RequestReport(location);
                return Ok(reportId);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, log or return specific error response
                return BadRequest($"Failed to request report. Error: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Report>> GetReportDetails(string reportId)
        {
            try
            {
                var reportDetails = await _reportingService.GetReportDetails(reportId);
                if (reportDetails == null)
                {
                    return NotFound($"Report with ID {reportId} not found.");
                }
                return Ok(reportDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get report details. Error: {ex.Message}");
            }
        }
        
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ReportStatus>>> GetReportStatuses()
        {
            try
            {
                var reportStatuses = await _reportingService.GetReportStatuses();
                return Ok(reportStatuses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get report statuses. Error: {ex.Message}");
            }
        }
        
    }
}
