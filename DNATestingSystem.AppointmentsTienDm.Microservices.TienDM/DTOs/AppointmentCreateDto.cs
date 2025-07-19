namespace DNATestingSystem.AppointmentsTienDm.Microservices.TienDM.DTOs
{
    public class AppointmentCreateDto
    {
        public int UserAccountId { get; set; } = 1;
        public int ServicesNhanVtid { get; set; } = 1;
        public int AppointmentStatusesTienDmid { get; set; } = 1;
        public string AppointmentDate { get; set; } = "";
        public string AppointmentTime { get; set; } = "";
        public string SamplingMethod { get; set; } = "";
        public string? Address { get; set; }
        public string ContactPhone { get; set; } = "";
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class AppointmentUpdateDto
    {
        public int UserAccountId { get; set; }
        public int ServicesNhanVtid { get; set; }
        public int AppointmentStatusesTienDmid { get; set; }
        public string AppointmentDate { get; set; } = "";
        public string AppointmentTime { get; set; } = "";
        public string SamplingMethod { get; set; } = "";
        public string? Address { get; set; }
        public string ContactPhone { get; set; } = "";
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
