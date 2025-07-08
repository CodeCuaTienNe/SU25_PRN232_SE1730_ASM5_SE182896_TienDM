using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DNATestingSystem.BusinessObject.Shared.Model.TienDM.Models;

public partial class AppointmentsTienDm
{
    [Key]
    public int AppointmentsTienDmid { get; set; }

    [Required(ErrorMessage = "User Account is required")]
    [Display(Name = "User Account")]
    public int UserAccountId { get; set; }

    [Required(ErrorMessage = "Service selection is required")]
    [Display(Name = "Service")]
    public int ServicesNhanVtid { get; set; }
    [Required(ErrorMessage = "Appointment status is required")]
    [Display(Name = "Appointment Status")]
    public int AppointmentStatusesTienDmid { get; set; }

    [Required(ErrorMessage = "Appointment date is required")]
    [Display(Name = "Appointment Date")]
    [DataType(DataType.Date)]
    public DateOnly AppointmentDate { get; set; }

    [Required(ErrorMessage = "Appointment time is required")]
    [Display(Name = "Appointment Time")]
    [DataType(DataType.Time)]
    public TimeOnly AppointmentTime { get; set; }

    [Required(ErrorMessage = "Sampling method is required")]
    //[StringLength(100, MinimumLength = 3, ErrorMessage = "Sampling method must be between 3 and 100 characters")]
    //[Display(Name = "Sampling Method")]
    public string SamplingMethod { get; set; } = null!;

    [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Contact phone is required")]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Contact phone must be between 10 and 15 characters")]
    [Display(Name = "Contact Phone")]
    public string ContactPhone { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    [Display(Name = "Created Date")]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Modified Date")]
    public DateTime? ModifiedDate { get; set; }

    [Required(ErrorMessage = "Total amount is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Total amount must be between 0.01 and 999,999.99")]
    [DataType(DataType.Currency)]
    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Is Paid")]
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
