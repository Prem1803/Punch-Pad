using System;
using System.Collections.Generic;

namespace PunchPadServer.Models
{
    public partial class PremAttendance
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? AttendanceDay { get; set; }
        public DateTime? AttendanceTime { get; set; }
        public string? AttendanceType { get; set; }
        public string? AttendanceStatus { get; set; }

        public virtual PremEmployee? Emp { get; set; }
        public virtual PremEmployee? Manager { get; set; }
    }
}
