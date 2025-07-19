using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;

public partial class AppointmentsTienDm
{
    public int AppointmentsTienDmid { get; set; }
    public int UserAccountId { get; set; }
    public int ServicesNhanVtid { get; set; }
    public int AppointmentStatusesTienDmid { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentTime { get; set; }
    public string SamplingMethod { get; set; } = null!;
    public string? Address { get; set; }
    public string ContactPhone { get; set; } = null!;
    public string? Notes { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public decimal TotalAmount { get; set; }

    public bool? IsPaid { get; set; }
    // Navigation properties - Remove JsonIgnore to allow serialization for display
    public virtual AppointmentStatusesTienDm? AppointmentStatusesTienDm { get; set; }

    //[JsonIgnore] // Keep this ignored as it's not needed for display
    //public virtual ICollection<SampleThinhLc> SampleThinhLcs { get; set; } = new List<SampleThinhLc>();

    //// Navigation properties - Remove JsonIgnore to allow serialization for display
    //public virtual ServicesNhanVt? ServicesNhanVt { get; set; }

    //// Navigation properties - Remove JsonIgnore to allow serialization for display  
    //public virtual SystemUserAccount? UserAccount { get; set; }
}
