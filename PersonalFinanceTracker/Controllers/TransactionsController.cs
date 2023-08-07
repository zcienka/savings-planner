using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceTracker.Dtos;
using PersonalFinanceTracker.ViewModels;

namespace PersonalFinanceTracker.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsController(ITransactionRepository transactionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int? categoryId, int pg = 1)
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

            IEnumerable<TransactionCategory> categories = await _transactionRepository.GetAllCategories();

            return View(new Tuple<IEnumerable<Transaction>, IEnumerable<TransactionCategory>>(data, categories));
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

        public async Task<IActionResult> Create()
        {
            var transactionDto = new TransactionDto
            {
                Date = DateTime.Now
            };
            var categories = await _transactionRepository.GetAllCategories();
            var types = await _transactionRepository.GetAllTypes();

            var viewModel = new TransactionCreateViewModel
            {
                TransactionDto = transactionDto,
                Categories = categories,
                Types = types
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionCreateViewModel createViewModel)
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var transactionDto = createViewModel.TransactionDto;

            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    Category = transactionDto.Category,
                    Date = new DateTime(
                        transactionDto.Date.Year,
                        transactionDto.Date.Month,
                        transactionDto.Date.Day
                    ),
                    Description = transactionDto.Description,
                    Amount = transactionDto.Amount,
                    Type = transactionDto.Type,
                    UserId = currUserId,
                };
                _transactionRepository.Add(transaction);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _transactionRepository.GetAllCategories();
            var types = await _transactionRepository.GetAllTypes();
            createViewModel.Categories = categories;
            createViewModel.Types = types;

            return View(createViewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var categories = await _transactionRepository.GetAllCategories();
            var types = await _transactionRepository.GetAllTypes();

            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            var viewModel = new TransactionEditViewModel
            {
                Transaction = transaction,
                Categories = categories,
                Types = types
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TransactionEditViewModel editViewModel)
        {
            var transaction = editViewModel.Transaction;

            if (id != transaction.Id)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
                return View(editViewModel);
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