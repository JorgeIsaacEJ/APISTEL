using APISTEL.DB;
using APISTEL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APISTEL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        public PersonaController() { }

        // GET api/<PersonaController>/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                var users = dBAccess.getExcelFile();

                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<PersonaController>/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                var users = dBAccess.getExcelFile(id);

                if(users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<PersonaController>/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Users user)
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                dBAccess.postExcelFile(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/<PersonaController>/
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] Users user)
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                dBAccess.updateExcelFile(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<PersonaController>/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                dBAccess.deleteExcelFile(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
