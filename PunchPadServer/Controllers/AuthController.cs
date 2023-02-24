using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchPadServer.Models;


namespace PunchPadServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ACE42023Context _context;

        public AuthController(ACE42023Context context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] PremEmployee user)
        {
            var Username = user.Username;
            var Password = user.Password;
            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            var empInDB = await _context.PremEmployees.SingleOrDefaultAsync((e) => e.Username == Username && e.Password == Password);
            if (empInDB == null)
                return BadRequest(new Exception("Invalid Username/Password"));
            return Ok(empInDB);
           
        }


        [HttpGet]
        [Route("IsManager")]
        public async Task<ActionResult> IsManger(int id)
        {

            if (_context.PremEmployees == null)
            {
                return NotFound();
            }
            
            var empInDB = await _context.PremEmployees.Where((e) => e.ManagerId == id).CountAsync();
            return Ok(empInDB);
            
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(PremEmployee user)
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

            return CreatedAtAction("Login", user);
        }


        private bool PremEmployeeExists(string Username)
        {
            return (_context.PremEmployees?.Any(e => e.Username == Username)).GetValueOrDefault();
        }

    }
}
