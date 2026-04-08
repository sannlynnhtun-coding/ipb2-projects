using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemMVC.Services.Result;
using IBP2.StudentResultManagementSystemMVC.Services.Student;
using IBP2.StudentResultManagementSystemMVC.Services.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBP2.StudentResultManagementSystemMVC.Controllers
{
    public class ResultController : Controller
    {
        private readonly IResultService _resultService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public ResultController(IResultService resultService, IStudentService studentService, ISubjectService subjectService)
        {
            _resultService = resultService;
            _studentService = studentService;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var response = await _resultService.GetResultsAsync(pageNo, pageSize);
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = response.TotalCount;
            return View(response.Data);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateResultRequest request)
        {
            if (ModelState.IsValid)
            {
                await _resultService.CreateResultAsync(request);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            UpdateResultRequest updateResultRequest = new()
            {
                Id = result.Id,
                StudentId = result.StudentId,
                SubjectId = result.SubjectId,
                Marks = (int)result.Marks
            };
            var request = updateResultRequest;

            await PopulateDropdowns();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateResultRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _resultService.UpdateResultAsync(request);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(request);
        }

        private async Task PopulateDropdowns()
        {
            var studentsResponse = await _studentService.GetStudentsAsync();
            var subjectsResponse = await _subjectService.GetSubjectsAsync();

            ViewBag.Students = new SelectList(studentsResponse.Data, "Id", "Name");
            ViewBag.Subjects = new SelectList(subjectsResponse.Data, "Id", "Name");
        }
    }
}
