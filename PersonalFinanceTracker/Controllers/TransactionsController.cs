using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace PersonalFinanceTracker.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsController(ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int pg=1)
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            IEnumerable<Transaction> transactions = await _transactionRepository.GetAllByUserId(currUserId);

            const int pageSize = 20;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = transactions.Count();

            var pager = new Pager(recsCount, pg, pageSize); 

            int recSkip = (pg - 1) * pageSize;
            var data = transactions
                .Skip(recSkip)
                .Take(pager.PageSize)   
                .ToList();
            this.ViewBag.Pager = pager;
            
            return View(data);
        }

        [HttpGet]
        public IActionResult GetTransactionsData()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            var chartData = _transactionRepository.GetIncomeAndExpensesPast12Months(currUserId);

            return Json(chartData); 
        }

        [HttpGet]
        public IActionResult GetIncomeByCategory()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            var categories = _transactionRepository.GetCategoriesByCurrentMonth("Income", currUserId);

            var amount = _transactionRepository.GetTransactionListByCurrentMonth("Income", currUserId);

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
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            var categories = _transactionRepository.GetCategoriesByCurrentMonth("Expense", currUserId);

            var amount = _transactionRepository.GetTransactionListByCurrentMonth("Expense", currUserId);

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
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            var expenses = _transactionRepository.GetSumByCurrentMonth("Expense", currUserId);

            var income = _transactionRepository.GetSumByCurrentMonth("Income", currUserId);

            return Json(new
            {
                Balance = income - expenses,
                Income = income,
                Expenses = expenses,
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

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
        public async Task<IActionResult> Create([Bind("Id,UserId,Category,Date,Description,Type,Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _transactionRepository.Add(transaction);
                return RedirectToAction(nameof(Index));
            }

            return View(transaction);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,UserId,Category,Date,Description,Type,Amount")]
            Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(transaction);
            try
            {
                _transactionRepository.Update(transaction);
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

        public async Task<IActionResult> Delete(string id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
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
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction != null)
            {
                _transactionRepository.Delete(transaction);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
            return _transactionRepository.Exists(id);
        }
    }
}