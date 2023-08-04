using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceTracker.Interfaces;

namespace PersonalFinanceTracker.Controllers
{
    [Authorize]
    public class SavingsController : Controller
    {
        private readonly ISavingsRepository _savingsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SavingsController(ISavingsRepository savingsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _savingsRepository = savingsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            IEnumerable<Savings> savings = await _savingsRepository.GetAll(currUserId);

            const int pageSize = 20;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = savings.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;
            var data = savings
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToList();
            this.ViewBag.Pager = pager;

            return View(savings);
        }

        public async Task<IActionResult> Details(string id)
        {
            var savings = await _savingsRepository.GetByIdAsync(id);

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
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Savings savings)
        {
            if (ModelState.IsValid)
            {
                _savingsRepository.Add(savings);
                return RedirectToAction(nameof(Index));
            }

            return View(savings);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var savings = await _savingsRepository.GetByIdAsync(id);

            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")] Savings savings)
        {
            if (id != savings.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(savings);
            try
            {
                _savingsRepository.Update(savings);
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

        public async Task<IActionResult> Delete(string id)
        {
            var savings = await _savingsRepository.GetByIdAsync(id);
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
            var savings = await _savingsRepository.GetByIdAsync(id);
            if (savings != null)
            {
                _savingsRepository.Delete(savings);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SavingsExists(string id)
        {
            return _savingsRepository.Exists(id);
        }
    }
}