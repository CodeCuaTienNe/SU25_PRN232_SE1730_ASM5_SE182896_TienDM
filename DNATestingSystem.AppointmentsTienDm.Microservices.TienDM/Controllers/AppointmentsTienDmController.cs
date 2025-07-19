using Microsoft.AspNetCore.Mvc;
using DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;
using DNATestingSystem.Common.Shared.TienDM;
using MassTransit;
using DNATestingSystem.AppointmentsTienDm.Microservices.TienDM.DTOs;
using AppointmentModel = DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models.AppointmentsTienDm;

namespace DNATestingSystem.AppointmentsTienDm.Microservices.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsTienDmController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<AppointmentsTienDmController> _logger;
        private static List<AppointmentModel> _appointments = new List<AppointmentModel>
        {
            new AppointmentModel
            {
                AppointmentsTienDmid = 1,
                UserAccountId = 1,
                ServicesNhanVtid = 1,
                AppointmentStatusesTienDmid = 1,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                AppointmentTime = new TimeOnly(14, 30, 0),
                SamplingMethod = "Blood",
                Address = "123 Main St",
                ContactPhone = "0123456789",
                Notes = "Sample appointment",
                TotalAmount = 150.00m,
                IsPaid = false,
                CreatedDate = DateTime.Now
            },
            new AppointmentModel
            {
                AppointmentsTienDmid = 2,
                UserAccountId = 2,
                ServicesNhanVtid = 2,
                AppointmentStatusesTienDmid = 2,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                AppointmentTime = new TimeOnly(10, 0, 0),
                SamplingMethod = "Saliva",
                Address = "456 Oak Ave",
                ContactPhone = "0987654321",
                Notes = "Follow-up test",
                TotalAmount = 200.00m,
                IsPaid = true,
                CreatedDate = DateTime.Now
            }
        };

        public AppointmentsTienDmController(IBus bus, ILogger<AppointmentsTienDmController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        // GET: api/AppointmentsTienDm
        [HttpGet]
        public ActionResult<IEnumerable<AppointmentModel>> GetAppointments()
        {
            _logger.LogInformation("Getting all appointments");
            return Ok(_appointments);
        }

        // GET: api/AppointmentsTienDm/5
        [HttpGet("{id}")]
        public ActionResult<AppointmentModel> GetAppointment(int id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentsTienDmid == id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment with ID {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved appointment with ID {id}");
            return Ok(appointment);
        }

        // POST: api/AppointmentsTienDm
        [HttpPost]
        public async Task<ActionResult<AppointmentModel>> PostAppointment(AppointmentCreateDto appointmentDto)
        {
            try
            {
                // Convert DTO to model with proper date/time conversion
                var appointment = new AppointmentModel
                {
                    UserAccountId = appointmentDto.UserAccountId,
                    ServicesNhanVtid = appointmentDto.ServicesNhanVtid,
                    AppointmentStatusesTienDmid = appointmentDto.AppointmentStatusesTienDmid,
                    AppointmentDate = DateOnly.Parse(appointmentDto.AppointmentDate),
                    AppointmentTime = TimeOnly.Parse(appointmentDto.AppointmentTime),
                    SamplingMethod = appointmentDto.SamplingMethod,
                    Address = appointmentDto.Address,
                    ContactPhone = appointmentDto.ContactPhone,
                    Notes = appointmentDto.Notes,
                    TotalAmount = appointmentDto.TotalAmount,
                    IsPaid = appointmentDto.IsPaid,
                    CreatedDate = DateTime.Now
                };

                // Set ID and timestamps
                appointment.AppointmentsTienDmid = _appointments.Count > 0 ? _appointments.Max(a => a.AppointmentsTienDmid) + 1 : 1;

                // Add to list
                _appointments.Add(appointment);

                // Send message to RabbitMQ
                await _bus.Publish(appointment);

                string messageLog = string.Format("[{0}] SEND DATA to Queue: {1}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Utilities.ConvertObjectToJsonString(appointment));

                Utilities.WriteLoggerFile(messageLog);
                _logger.LogInformation(messageLog);

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentsTienDmid }, appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/AppointmentsTienDm/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, AppointmentModel appointment)
        {
            var existingAppointment = _appointments.FirstOrDefault(a => a.AppointmentsTienDmid == id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            // Update properties
            existingAppointment.UserAccountId = appointment.UserAccountId;
            existingAppointment.ServicesNhanVtid = appointment.ServicesNhanVtid;
            existingAppointment.AppointmentStatusesTienDmid = appointment.AppointmentStatusesTienDmid;
            existingAppointment.AppointmentDate = appointment.AppointmentDate;
            existingAppointment.AppointmentTime = appointment.AppointmentTime;
            existingAppointment.SamplingMethod = appointment.SamplingMethod;
            existingAppointment.Address = appointment.Address;
            existingAppointment.ContactPhone = appointment.ContactPhone;
            existingAppointment.Notes = appointment.Notes;
            existingAppointment.TotalAmount = appointment.TotalAmount;
            existingAppointment.IsPaid = appointment.IsPaid;
            existingAppointment.ModifiedDate = DateTime.Now;

            // Send message to RabbitMQ
            await _bus.Publish(existingAppointment);

            string messageLog = string.Format("[{0}] PUBLISH updated data to RabbitMQ.appointmentsTienDmQueue: {1}",
                DateTime.Now.ToString(),
                Utilities.ConvertObjectToJsonString(existingAppointment));

            Utilities.WriteLoggerFile(messageLog);
            _logger.LogInformation(messageLog);

            return NoContent();
        }

        // DELETE: api/AppointmentsTienDm/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentsTienDmid == id);
            if (appointment == null)
            {
                return NotFound();
            }

            _appointments.Remove(appointment);

            _logger.LogInformation($"Deleted appointment with ID {id}");
            return NoContent();
        }
    }
}
