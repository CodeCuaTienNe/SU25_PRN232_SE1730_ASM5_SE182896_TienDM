using Microsoft.AspNetCore.Mvc;
using DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;
using StatusModel = DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models.AppointmentStatusesTienDm;

namespace DNATestingSystem.AppointmentStatusesTienDm.Microservices.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentStatusesTienDmController : ControllerBase
    {
        private readonly ILogger<AppointmentStatusesTienDmController> _logger;
        private static List<StatusModel> _statuses = new List<StatusModel>
        {
            new StatusModel { AppointmentStatusesTienDmid = 1, StatusName = "Pending", Description = "Appointment is pending", IsActive = true, CreatedDate = DateTime.Now },
            new StatusModel { AppointmentStatusesTienDmid = 2, StatusName = "Confirmed", Description = "Appointment is confirmed", IsActive = true, CreatedDate = DateTime.Now },
            new StatusModel { AppointmentStatusesTienDmid = 3, StatusName = "Completed", Description = "Appointment is completed", IsActive = true, CreatedDate = DateTime.Now },
            new StatusModel { AppointmentStatusesTienDmid = 4, StatusName = "Cancelled", Description = "Appointment is cancelled", IsActive = true, CreatedDate = DateTime.Now }
        };

        public AppointmentStatusesTienDmController(ILogger<AppointmentStatusesTienDmController> logger)
        {
            _logger = logger;
        }

        // GET: api/AppointmentStatusesTienDm
        [HttpGet]
        public ActionResult<IEnumerable<StatusModel>> GetStatuses()
        {
            _logger.LogInformation("Getting all appointment statuses");
            return Ok(_statuses.Where(s => s.IsActive == true));
        }

        // GET: api/AppointmentStatusesTienDm/5
        [HttpGet("{id}")]
        public ActionResult<StatusModel> GetStatus(int id)
        {
            var status = _statuses.FirstOrDefault(s => s.AppointmentStatusesTienDmid == id);
            if (status == null)
            {
                _logger.LogWarning($"Status with ID {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved status with ID {id}");
            return Ok(status);
        }

        // POST: api/AppointmentStatusesTienDm
        [HttpPost]
        public ActionResult<StatusModel> PostStatus(StatusModel status)
        {
            try
            {
                // Set ID and timestamps
                status.AppointmentStatusesTienDmid = _statuses.Count > 0 ? _statuses.Max(s => s.AppointmentStatusesTienDmid) + 1 : 1;
                status.CreatedDate = DateTime.Now;
                status.IsActive = true;

                // Add to list
                _statuses.Add(status);

                _logger.LogInformation($"Created new status with ID {status.AppointmentStatusesTienDmid}");

                return CreatedAtAction(nameof(GetStatus), new { id = status.AppointmentStatusesTienDmid }, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating status");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/AppointmentStatusesTienDm/5
        [HttpPut("{id}")]
        public IActionResult PutStatus(int id, StatusModel status)
        {
            var existingStatus = _statuses.FirstOrDefault(s => s.AppointmentStatusesTienDmid == id);
            if (existingStatus == null)
            {
                return NotFound();
            }

            // Update properties
            existingStatus.StatusName = status.StatusName;
            existingStatus.Description = status.Description;
            existingStatus.IsActive = status.IsActive;

            _logger.LogInformation($"Updated status with ID {id}");

            return NoContent();
        }

        // DELETE: api/AppointmentStatusesTienDm/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStatus(int id)
        {
            var status = _statuses.FirstOrDefault(s => s.AppointmentStatusesTienDmid == id);
            if (status == null)
            {
                return NotFound();
            }

            // Soft delete by setting IsActive to false
            status.IsActive = false;

            _logger.LogInformation($"Soft deleted status with ID {id}");
            return NoContent();
        }
    }
}
