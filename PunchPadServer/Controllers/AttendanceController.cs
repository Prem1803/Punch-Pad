using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchPadServer.Models;

namespace PunchPadServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ACE42023Context _context;

        public AttendanceController(ACE42023Context context)
        {
            _context = context;
        }
        [HttpGet("{userId}/{date}")]
        public async Task<ActionResult<IEnumerable<PremAttendance>>> GetAttendances(int userId, string date)
        {
            if (_context.PremAttendances == null)
            {
                return NotFound();
            }

            var attendances = await _context.PremAttendances.Where((a) => a.EmpId == userId && a.AttendanceDay == Convert.ToDateTime(date)).ToListAsync();

            return Ok(attendances);
        }


        [HttpGet("Team-Attendance/{userId}/{date}")]
        public async Task<ActionResult<IEnumerable<PremAttendance>>> GetTeamAttendances(int userId, string date, string keyword)
        {
            if (_context.PremAttendances == null)
            {
                return NotFound();
            }
            if (keyword == "null")
                keyword = "";
            var attendances = await _context.PremAttendances.Include(a => a.Emp).Where((a) => a.ManagerId == userId && a.AttendanceDay == Convert.ToDateTime(date) && a.AttendanceStatus == "Pending" && a.Emp.Name.Contains(keyword)).ToListAsync();

            return Ok(attendances);
        }


        private PremAttendance GenerateAttendance(int id, string type)
        {
            PremEmployee? e = _context.PremEmployees.Where((e) => e.Id == id).SingleOrDefault();
            PremAttendance attendance = new PremAttendance();
            attendance.EmpId = e.Id;
            attendance.ManagerId = e.ManagerId;
            attendance.AttendanceDay = DateTime.Now;
            attendance.AttendanceTime = DateTime.Now;
            attendance.AttendanceStatus = "Pending";
            attendance.AttendanceType = type;
            return attendance;
        }


        [HttpPost("{userId}/{type}")]
        public async Task<ActionResult> Punch(int userId, string type)
        {
            if (_context.PremAttendances == null)
            {
                return NotFound();
            }

            _context.PremAttendances.Add(GenerateAttendance(userId, type));
            await _context.SaveChangesAsync();
            return Ok($"Punch {type} Successful.");
        }


        [HttpPut("{id}/{type}")]
        public async Task<ActionResult> Update(int id, string type)
        {
            if (_context.PremAttendances == null)
            {
                return NotFound();
            }

            PremAttendance? p = await _context.PremAttendances.FindAsync(id);
            if (p == null)
                return NoContent();

            p.AttendanceStatus = type;
            await _context.SaveChangesAsync();
            return Ok($"Attendance {type}.");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_context.PremAttendances == null)
            {
                return NotFound();
            }

            PremAttendance? p = await _context.PremAttendances.FindAsync(id);
            if (p == null)
                return NoContent();
            _context.PremAttendances.Remove(p);
            await _context.SaveChangesAsync();
            return Ok("Attendance Deleted.");
        }
    }
}
