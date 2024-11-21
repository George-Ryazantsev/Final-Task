using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trenning_NotificationsExample.Models;

namespace Trenning_NotificationsExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassportsController : Controller
    {
        private readonly PassportContext _context;

        public PassportsController(PassportContext context)
        {
            _context = context;
        }

        [HttpGet("{series}/{number}")]
        public async Task<ActionResult<InactivePassport>> GetInactivePassport(string series, string number)
        {
            var passport = await _context.InactivePassports.FirstOrDefaultAsync(p => p.Series == series && p.Number == number);

            if (passport == null)
            {
                return NotFound();
            }

            return passport;
        }

        [HttpGet("changes/{date}")]
        public async Task<ActionResult<IEnumerable<PassportChange>>> GetChangesByDate(DateTime date)
        {
            var changes = await _context.PassportChanges
                .Where(c => c.ChangeDate.Date == date.Date)
                .ToListAsync();

            if (!changes.Any())
            {
                return NotFound();
            }

            return changes;
        }

        [HttpGet("history/{series}/{number}")]
        public async Task<ActionResult<IEnumerable<PassportChange>>> GetPassportHistory(string series, string number)
        {
            var history = await _context.PassportChanges
                .Where(c => c.Series == series && c.Number == number)
                .ToListAsync();

            if (!history.Any())
            {
                return NotFound();
            }

            return history;
        }
    }
}
