using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PunchPadServer.Models
{
    public partial class PremEmployee
    {
        public PremEmployee()
        {
            InverseManager = new HashSet<PremEmployee>();
            PremAttendanceEmps = new HashSet<PremAttendance>();
            PremAttendanceManagers = new HashSet<PremAttendance>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        [Display(Name ="Date of Joining")]
        public DateTime? Doj { get; set; }
        [Required(ErrorMessage = "Please enter the username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter the password")]
        public string? Password { get; set; }
        public int? ManagerId { get; set; }

        public virtual PremEmployee? Manager { get; set; }
        public virtual ICollection<PremEmployee> InverseManager { get; set; }
        public virtual ICollection<PremAttendance> PremAttendanceEmps { get; set; }
        public virtual ICollection<PremAttendance> PremAttendanceManagers { get; set; }
    }
}
