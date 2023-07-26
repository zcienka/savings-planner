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
            List<Transaction> transactions = _context.Transactions.ToList();

            var dates = transactions.Select(t => t.Date.ToString("yyyy-MM-dd")).ToList();
            var amount = transactions.Select(t => t.Amount).ToList();

            var chartData = new
            {
                Dates = dates,
                Amount = amount
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult GetExpenses()
        {
            List<Transaction> transactions = _context.Transactions.Where(t => t.Type == "Expense").ToList();

            var dates = transactions
                .Select(t => t.Date.ToString("yyyy-MM-dd"))
                .ToList();
            var amount = transactions
                .Select(t => t.Amount)
                .ToList();

            var chartData = new
            {
                Dates = dates,
                Amount = amount
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult GetIncome()
        {
            List<Transaction> transactions = _context.Transactions
                .Where(t => t.Type == "Income")
                .ToList();

            var dates = transactions
                .Select(t => t.Date.ToString("yyyy-MM-dd"))
                .ToList();
            var income = transactions
                .Select(t => t.Amount)
                .ToList();

            var chartData = new
            {
                Dates = dates,
                Amount = income
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult GetTransactionsByCategory()
        {
            var categories = _context.Transactions
                .GroupBy(t => t.Category)
                .Select(t => t.Key);

            var amount = _context.Transactions
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
        public IActionResult GetTransactionsPreview()
        {
            var transactions = _context.Transactions
                .Take(10)
                .ToList();

            return Json(transactions);
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