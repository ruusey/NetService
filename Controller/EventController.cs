using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class EventController : ControllerBase
    {
        private readonly IEventReaderService _context;

        public EventController(IEventReaderService context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public  HashSet<EventLogEntry> GetEventLogs()
        {
           
            return _context.getEntrysByLogGroup("Application",100);
        }

    }
}
