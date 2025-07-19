namespace Micro1.Client.MauiHybridApp.TienDM.Models
{
    public class AppointmentCreateRequest
    {
        public int UserAccountId { get; set; } = 1;
        public int ServicesNhanVtid { get; set; } = 1;
        public int AppointmentStatusesTienDmid { get; set; } = 1;
        public string AppointmentDate { get; set; } = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        public string AppointmentTime { get; set; } = "14:30:00";
        public string SamplingMethod { get; set; } = "";
        public string? Address { get; set; } = "";
        public string ContactPhone { get; set; } = "";
        public string? Notes { get; set; } = "";
        public decimal TotalAmount { get; set; } = 150.00m;
        public bool IsPaid { get; set; } = false;
    }

    public class AppointmentResponse
    {
        public int AppointmentsTienDmid { get; set; }
        public int UserAccountId { get; set; }
        public int ServicesNhanVtid { get; set; }
        public int AppointmentStatusesTienDmid { get; set; }
        public string AppointmentDate { get; set; } = "";
        public string AppointmentTime { get; set; } = "";
        public string SamplingMethod { get; set; } = "";
        public string? Address { get; set; }
        public string ContactPhone { get; set; } = "";
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
