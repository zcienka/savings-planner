using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Repository;

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

            IEnumerable<Savings> savings = await _savingsRepository.GetAllByUserId(currUserId);

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

            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            var savings = new Savings(); 
            var savingsStatus = await _savingsRepository.GetSavingsStatus();

            return View(new Tuple<Savings, IEnumerable<SavingsStatus>>(savings, savingsStatus));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")]
            Savings savings)
        {
            IEnumerable<SavingsStatus> savingsStatus = await _savingsRepository.GetSavingsStatus();

            if (ModelState.IsValid)
            {
                _savingsRepository.Add(savings);
                return RedirectToAction(nameof(Index));
            }

            return View(new Tuple<Savings, IEnumerable<SavingsStatus>>(savings, savingsStatus)); 
        }

        public async Task<IActionResult> Edit(string id)
        {
            var savings = await _savingsRepository.GetByIdAsync(id);

            if (savings == null)
            {
                return NotFound();
            }
            IEnumerable<SavingsStatus> savingsStatus = await _savingsRepository.GetSavingsStatus();

            return View(new Tuple<Savings, IEnumerable<SavingsStatus>>(savings, savingsStatus));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,UserId,TargetAmount,CurrentAmount,Deadline,Status")]
            Savings savings)
        {
            IEnumerable<SavingsStatus> savingsStatus = await _savingsRepository.GetSavingsStatus();

            if (id != savings.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(new Tuple<Savings, IEnumerable<SavingsStatus>>(savings, savingsStatus));
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