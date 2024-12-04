using Microsoft.AspNetCore.Mvc;
using Trenning_NotificationsExample.Models;
using Trenning_NotificationsExample.Services;

namespace Trenning_NotificationsExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassportsController : ControllerBase
    {
        private readonly PassportUpdateService _passportUpdateService;

        public PassportsController(PassportUpdateService passportUpdateService)
        {
            _passportUpdateService = passportUpdateService;
        }
        
        [HttpGet("{series}/{number}")]
        public async Task<ActionResult<PassportChange>> GetInactivePassport(string series, string number)
        {
            var changes = await _passportUpdateService.LoadChangesAsync();
           
            var lastChange = changes
                .Where(c => c.Series == series && c.Number == number)
                .OrderByDescending(c => c.ChangeDate)
                .FirstOrDefault();

            /*if (lastChange == null || lastChange.ChangeType == "Removed")
            {
                return NotFound(); //404 
            }*/

            return lastChange;
        }
        
        [HttpGet("changes/{date}")]
        public async Task<ActionResult<IEnumerable<PassportChange>>> GetChangesByDate(DateTime date)
        {
            var changes = await _passportUpdateService.LoadChangesAsync();
            
            var changesByDate = changes
                .Where(c => c.ChangeDate.Date == date.Date)
                .ToList();

            if (!changesByDate.Any())
            {
                return NotFound();
            }

            return changesByDate;
        }
        
        [HttpGet("history/{series}/{number}")]
        public async Task<ActionResult<IEnumerable<PassportChange>>> GetPassportHistory(string series, string number)
        {
            var changes = await _passportUpdateService.LoadChangesAsync();
            
            var history = changes
                .Where(c => c.Series == series && c.Number == number)
                .OrderBy(c => c.ChangeDate)
                .ToList();

            if (!history.Any())
            {
                return NotFound();
            }

            return history;
        }
    }
}
