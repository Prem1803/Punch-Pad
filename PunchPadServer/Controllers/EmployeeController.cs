using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchPadServer.Models;

namespace PunchPadServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ACE42023Context _context;

        public EmployeeController(ACE42023Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PremEmployee>>> GetPremEmployees()
        {
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            return Ok(await _context.PremEmployees.ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PremEmployee>> GetPremEmployee(int id)
        {
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            var empInDB = await _context.PremEmployees.Include(a => a.Manager).SingleOrDefaultAsync(a => a.Id == id);

            if (empInDB == null)
            {
                return NotFound();
            }

            return Ok(empInDB);
        }


        [HttpGet("ByUserName/{username}")]
        public async Task<ActionResult<PremEmployee>> GetPremEmployeeByUsername(string username)
        {
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            PremEmployee? empInDB = await _context.PremEmployees.Include(a => a.Manager).SingleOrDefaultAsync(e => e.Username == username);

            if (empInDB == null)
            {
                return NotFound();
            }

            return Ok(empInDB);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPremEmployee(int id, PremEmployee user)
        {

            if (id != user.Id)
            {
                return BadRequest();
            }
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (PremEmployeeExists(user.Username))
                {
                    return BadRequest(new Exception($"Username {user.Username} is already used. Try a different Username."));
                }
                else
                {
                    return BadRequest(e);
                }
            }

            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<PremEmployee>> PostPremEmployee(PremEmployee user)
        {
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (PremEmployeeExists(user.Username))
                {
                    return BadRequest(new Exception($"Username {user.Username} is already used. Try a different Username."));
                }
                else
                {
                    return BadRequest(e);
                }
            }

            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremEmployee(int id)
        {
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            var premEmployee = await _context.PremEmployees.FindAsync(id);
            if (premEmployee == null)
            {
                return NotFound();
            }
            try
            {
                _context.PremEmployees.Remove(premEmployee);
                await _context.SaveChangesAsync();

                return Ok("Employee Deleted");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


        private bool PremEmployeeExists(string Username)
        {
            return (_context.PremEmployees?.Any(e => e.Username == Username)).GetValueOrDefault();
        }
    }
}
