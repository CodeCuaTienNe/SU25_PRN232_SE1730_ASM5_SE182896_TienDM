using MassTransit;
using DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;
using DNATestingSystem.Common.Shared.TienDM;
using AppointmentModel = DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models.AppointmentsTienDm;

namespace DNATestingSystem.AppointmentStatusesTienDm.Microservices.TienDM.Consumers
{
    public class AppointmentsTienDmConsumer : IConsumer<AppointmentModel>
    {
        private readonly ILogger<AppointmentsTienDmConsumer> _logger;

        public AppointmentsTienDmConsumer(ILogger<AppointmentsTienDmConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentModel> context)
        {
            var appointment = context.Message;
            if (appointment != null)
            {
                string messageLog = string.Format("[{0}] RECEIVE DATA from Queue: {1}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Utilities.ConvertObjectToJsonString(appointment));
                Utilities.WriteLoggerFile(messageLog);
                _logger.LogInformation(messageLog);
            }
        }
    }
}
