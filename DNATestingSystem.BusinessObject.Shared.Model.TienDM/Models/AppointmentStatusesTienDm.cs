using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;

public partial class AppointmentStatusesTienDm
{
    public int AppointmentStatusesTienDmid { get; set; }

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }    // Keep JsonIgnore to avoid circular reference when serializing appointments collection
    [JsonIgnore]
    public virtual ICollection<AppointmentsTienDm> AppointmentsTienDms { get; set; } = new List<AppointmentsTienDm>();
}
