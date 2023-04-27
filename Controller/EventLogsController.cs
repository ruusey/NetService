using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetService.Models;
using NetService.Repo;
using NetService.Service;

namespace NetService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventLogsController : ControllerBase
    {
        private readonly EventLogRepo _context;

        public EventLogsController(EventLogRepo context)
        {
            _context = context;
        }

        // GET: api/EventLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventLog>>> GetEvents(int page, int size)
        {
          if (_context.Events == null)
          {
              return NotFound();
          }
            if (size == 0)
            {
                size = 10;
            }
          return await _context.Events.Skip(page*size).Take(size).ToListAsync();
          //  return await _context.Events.ToListAsync();
        }

        // GET: api/EventLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventLog>> GetEventLog(int id)
        {
          if (_context.Events == null)
          {
              return NotFound();
          }
            var eventLog = await _context.Events.FindAsync(id);

            if (eventLog == null)
            {
                return NotFound();
            }

            return eventLog;
        }

        [HttpGet("source/{id}")]
        public async Task<ActionResult<IEnumerable<EventLog>>> GetEventLogsBySource(string id, int page, int size)
        {
            if (_context.Events == null || !EventReaderService.LOG_NAMES.Contains(id))
            {
                return NotFound();
            }
            String queryStr = "SELECT * FROM EventLog WHERE source = {0}";
            var eventLog = await (_context.Events.Where(entry=>entry.Source == id).Skip(page * size).Take(size).ToListAsync());

            if (eventLog == null)
            {
                return NotFound();
            }

            return eventLog;
        }

        // PUT: api/EventLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventLog(int id, EventLog eventLog)
        {
            if (id != eventLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventLogExists(id))
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

        // POST: api/EventLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventLog>> PostEventLog(EventLog eventLog)
        {
          if (_context.Events == null)
          {
              return Problem("Entity set 'EventLogRepo.Events'  is null.");
          }
            _context.Events.Add(eventLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventLog", new { id = eventLog.Id }, eventLog);
        }

        // DELETE: api/EventLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventLog(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var eventLog = await _context.Events.FindAsync(id);
            if (eventLog == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventLogExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
