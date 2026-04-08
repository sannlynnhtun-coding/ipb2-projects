using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemMVC.Services.Student;
using Microsoft.AspNetCore.Mvc;

namespace IBP2.StudentResultManagementSystemMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _studentService.GetStudentsAsync();
            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentRequest request)
        {
            if (ModelState.IsValid)
            {
                await _studentService.CreateStudentAsync(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _studentService.GetStudentByIdAsync(id);
            if (response == null || response.Data == null)
            {
                return NotFound();
            }

            var request = new UpdateStudentRequest
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Email = response.Data.Email
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateStudentRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _studentService.UpdateStudentAsync(request);
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
            var success = await _studentService.DeleteStudentAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
