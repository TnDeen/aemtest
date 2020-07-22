using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aemtest.Contexts;
using aemtest.Models;
using aemtest.Dto;
using AutoMapper;

namespace aemtest.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class WellsController : ControllerBase
    {
        private readonly AemContext _context;
        private readonly IMapper _mapper;

        public WellsController(AemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Wells
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Well>>> GetWells()
        {
            return await _context.Wells.ToListAsync();
        }

        // GET: api/Wells/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Well>> GetWell(int id)
        {
            var well = await _context.Wells.FindAsync(id);

            if (well == null)
            {
                return NotFound();
            }

            return well;
        }

        // PUT: api/Wells/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWell(int id, Well well)
        {
            if (id != well.Id)
            {
                return BadRequest();
            }

            _context.Entry(well).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WellExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Wells
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Well>> PostWell(WellDto wellDto)
        {
            var well = _mapper.Map(wellDto, new Well());
            _context.Wells.Add(well);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWell", new { id = well.Id }, well);
        }

        // DELETE: api/Wells/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Well>> DeleteWell(int id)
        {
            var well = await _context.Wells.FindAsync(id);
            if (well == null)
            {
                return NotFound();
            }

            _context.Wells.Remove(well);
            await _context.SaveChangesAsync();

            return well;
        }

        private bool WellExists(int id)
        {
            return _context.Wells.Any(e => e.Id == id);
        }
    }
}
