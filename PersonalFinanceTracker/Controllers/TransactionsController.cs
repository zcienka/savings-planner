using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Transactions != null
                ? View(await _context.Transactions.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        [HttpGet]
        public IActionResult GetTransactionsData()
        {
            var chartData = _context.Transactions
                .GroupBy(t => new { Month = t.Date.Month, Year = t.Date.Year })
                .Select(group => new
                {
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    Income = group.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = group.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(group => group.Year)
                .ThenBy(group => group.Month)
                .ToList();
     
            return Json(chartData); 
        }

        [HttpGet]
        public IActionResult GetIncomeByCategory()
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            var categories = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Income")
                .GroupBy(t => t.Category)
                .Select(t => t.Key);

            var amount = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Income")
                .GroupBy(t => t.Category)
                .Select(t => t.Sum(t => t.Amount));

            var chartData = new
            {
                Category = categories,
                Amount = amount
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult GetExpensesByCategory()
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            var categories = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Expense")
                .GroupBy(t => t.Category)
                .Select(t => t.Key);

            var amount = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Expense")
                .GroupBy(t => t.Category)
                .Select(t => t.Sum(t => t.Amount));

            var chartData = new
            {
                Category = categories,
                Amount = amount
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult GetMonthlyBalance()
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;
            var expenses = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Expense")
                .Sum(t => t.Amount);

            var income = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == "Income")
                .ToList()
                .Sum(t => t.Amount);

            return Json(new
            {
                Balance = expenses + income,
                Income = income,
                Expenses = expenses,
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Category,Date,Description")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(transaction);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,UserId,Category,Date,Description")]
            Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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

            return View(transaction);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}