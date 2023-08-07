using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.ViewModels;
using PersonalFinanceTracker.Dtos;

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
            var savingsDto = new SavingsDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Deadline = DateOnly.FromDateTime(DateTime.Now),
            };
            var savingsStatus = await _savingsRepository.GetSavingsStatus();
            var viewModel = new SavingsCreateViewModel
            {
                SavingsDto = savingsDto,
                Status = savingsStatus
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavingsCreateViewModel createViewModel)
        {
            var savingsDto = createViewModel.SavingsDto;
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            if (ModelState.IsValid)
            {
                var savings = new Savings
                {
                    UserId = currUserId,
                    Title = savingsDto.Title,
                    Date = new DateTime(
                        savingsDto.Date.Year,
                        savingsDto.Date.Month,
                        savingsDto.Date.Day
                    ).ToUniversalTime(),
                    TargetAmount = savingsDto.TargetAmount,
                    CurrentAmount = savingsDto.CurrentAmount,
                    Deadline = new DateTime(
                        savingsDto.Deadline.Year,
                        savingsDto.Deadline.Month,
                        savingsDto.Deadline.Day
                    ).ToUniversalTime(),
                    Status = savingsDto.Status
                };
                _savingsRepository.Add(savings);
                return RedirectToAction(nameof(Index));
            }

            return View(createViewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var savings = await _savingsRepository.GetByIdAsync(id);

            if (savings == null)
            {
                return NotFound();
            }

            IEnumerable<SavingsStatus> savingsStatus = await _savingsRepository.GetSavingsStatus();

            var viewModel = new SavingsEditViewModel
            {
                Savings = savings,
                Status = savingsStatus
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SavingsEditViewModel editViewModel)
        {
            var savings = editViewModel.Savings;

            if (id != savings.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(editViewModel);
            try
            {
                var updatedSavings = new Savings
                {
                    Id = savings.Id,
                    UserId = savings.UserId,
                    Title = savings.Title,
                    Date = new DateTime(
                        savings.Date.Year,
                        savings.Date.Month,
                        savings.Date.Day
                    ).ToUniversalTime(),
                    TargetAmount = savings.TargetAmount,
                    CurrentAmount = savings.CurrentAmount,
                    Deadline = new DateTime(
                        savings.Deadline.Year,
                        savings.Deadline.Month,
                        savings.Deadline.Day
                    ).ToUniversalTime(),
                    Status = savings.Status
                };
                _savingsRepository.Update(updatedSavings);
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