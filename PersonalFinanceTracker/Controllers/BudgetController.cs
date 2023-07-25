using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using System.Linq;

namespace PersonalFinanceTracker.Controllers
{
    public class BudgetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.BudgetContext != null ? 
                          View(await _context.BudgetContext.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BudgetContext'  is null.");
        }

   

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BudgetContext == null)
            {
                return NotFound();
            }

            var savings = await _context.BudgetContext
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
        public async Task<IActionResult> Create([Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BudgetContext == null)
            {
                return NotFound();
            }

            var savings = await _context.BudgetContext.FindAsync(id);
            if (savings == null)
            {
                return NotFound();
            }
            return View(savings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingsExists(budget.Id))
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
            return View(budget);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BudgetContext == null)
            {
                return NotFound();
            }

            var savings = await _context.BudgetContext
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
            if (_context.BudgetContext == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BudgetContext'  is null.");
            }
            var savings = await _context.BudgetContext.FindAsync(id);
            if (savings != null)
            {
                _context.BudgetContext.Remove(savings);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavingsExists(string id)
        {
          return (_context.BudgetContext?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
