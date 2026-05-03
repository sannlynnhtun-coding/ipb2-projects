using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemMVC.Services.Subject;
using Microsoft.AspNetCore.Mvc;

namespace IBP2.StudentResultManagementSystemMVC.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _subjectService.GetSubjectsAsync();
            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubjectRequest request)
        {
            if (ModelState.IsValid)
            {
                await _subjectService.CreateSubjectAsync(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _subjectService.GetSubjectByIdAsync(id);
            if (response == null || response.Data == null)
            {
                return NotFound();
            }

            var request = new UpdateSubjectRequest
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Credits = response.Data.Credits
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSubjectRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _subjectService.UpdateSubjectAsync(request);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _subjectService.DeleteSubjectAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
