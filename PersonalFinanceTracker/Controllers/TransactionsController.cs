using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Transaction> transaction = await _transactionRepository.GetAll();

            return View(transaction);
        }

        [HttpGet]
        public IActionResult GetTransactionsData()
        {
            var chartData = _transactionRepository.GetIncomeAndExpenses();

            return Json(chartData); 
        }

        [HttpGet]
        public IActionResult GetIncomeByCategory()
        {
            var categories = _transactionRepository.GetCategoriesByCurrentMonth("Income");

            var amount = _transactionRepository.GetTransactionListByCurrentMonth("Income");

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
            var categories = _transactionRepository.GetCategoriesByCurrentMonth("Expense");

            var amount = _transactionRepository.GetTransactionListByCurrentMonth("Expense");

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
            var expenses = _transactionRepository.GetSumByCurrentMonth("Expense");

            var income = _transactionRepository.GetSumByCurrentMonth("Income");

            return Json(new
            {
                Balance = expenses + income,
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