using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class SavingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SavingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Savings != null ? 
                          View(await _context.Savings.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Savings'  is null.");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Savings == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Savings savings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(savings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(savings);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Savings == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings.FindAsync(id);
            if (savings == null)
            {
                return NotFound();
            }
            return View(savings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Savings savings)
        {
            if (id != savings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(savings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingsExists(savings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(savings);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Savings == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Savings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Savings'  is null.");
            }
            var savings = await _context.Savings.FindAsync(id);
            if (savings != null)
            {
                _context.Savings.Remove(savings);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavingsExists(string id)
        {
          return (_context.Savings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
