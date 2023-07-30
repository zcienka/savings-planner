using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceTracker.Interfaces;

namespace PersonalFinanceTracker.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetController(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Budget> budget = await _budgetRepository.GetAll();

            return View(budget);
        }

        public async Task<IActionResult> Details(string id)
        {
            var savings = await _budgetRepository.GetByIdAsync(id);

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
        public async Task<IActionResult> Create(
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                _budgetRepository.Add(budget);
                return RedirectToAction(nameof(Index));
            }

            return View(budget);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var savings = await _budgetRepository.GetByIdAsync(id);

            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(budget);
            try
            {
                _budgetRepository.Update(budget);
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

        public async Task<IActionResult> Delete(string id)
        {
            var savings = await _budgetRepository.GetByIdAsync(id);
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
            var savings = await _budgetRepository.GetByIdAsync(id);
            if (savings != null)
            {
                _budgetRepository.Delete(savings);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SavingsExists(string id)
        {
            return _budgetRepository.Exists(id);
        }
    }
}