using Game.Data;
using Game.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpelController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spel>>> GetSpel()
        {
            return await _context.Spel.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Spel>> GetSpel(int id)
        {
            var spel = await _context.Spel.FindAsync(id); 

            if (spel == null)
            {
                return NotFound();
            }

            return spel;
        }

        [HttpPost]
        public async Task<ActionResult<Spel>> PostSpel(Spel spel)
        {
            _context.Spel.Add(spel); 
            await _context.SaveChangesAsync(); 

            return CreatedAtAction(nameof(GetSpel), new { id = spel.Id }, spel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpel(int id, Spel spel)
        {
            if (id != spel.Id)
            {
                return BadRequest();
            }

            _context.Entry(spel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpel(int id)
        {
            var spel = await _context.Spel.FindAsync(id);
            if (spel == null)
            {
                return NotFound(); 
            }

            _context.Spel.Remove(spel); 
            await _context.SaveChangesAsync(); 

            return NoContent();
        }
    }
}
