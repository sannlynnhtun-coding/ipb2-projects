using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.WebApi.Features.User.Booking
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        IBookingService _bookService ;

        public BookingController(IBookingService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchBus(SearchBusRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Origin))
                return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "Origin is required." });
            if (string.IsNullOrWhiteSpace(request.Destination))
                return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "Destination is required." });
            //if (string.IsNullOrWhiteSpace(request.TravelDate))
            //    return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "Origin is required." });

            var response = await _bookService.SearchBus(request);
            return Ok(response);
        }

        [HttpPost("book")]
        public async Task<IActionResult> CreateBooking(BookRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ScheduleId))
                return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "Schedule Id is required." });

            if (request.Passengers == null || !request.Passengers.Any())
                return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "No passengers provided." });

            if (request.Passengers.Any(p => p.SeatNo <= 0 || string.IsNullOrWhiteSpace(p.Username) || string.IsNullOrWhiteSpace(p.Phoneno)))
                return BadRequest(new ResponseBaseModel { IsSuccess = false, Message = "Passenger details (Seat, Name, Phone) are required." });


            var response = await _bookService.CreateAsync(request);
            return ResponseHelper.ConvertResponseType(response);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(string id)
        {
            var result = await _bookService.CancelAsync(id);
            if (result.Status == ResponseType.Success)
            {
                return Ok(new ResponseBaseModel{ IsSuccess = true, Message = result.Message });
            }
            return Ok(new ResponseBaseModel { IsSuccess = false, Message = result.Message });
        }
        [HttpGet("getAllBookinglist")]
        public async Task<IActionResult> BookingAllList(string? txtBkSearch)
        {
            BookingDetailResponse response = new();
            response = await _bookService.GetBookingDetailAsync(txtBkSearch);            
            return Ok(response);
        }

        [HttpGet("getBookinglist")]
        public async Task<IActionResult> BookingListByUser(string? username, string? phoneno)
        {
            BookingDetailResponse response = new();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(phoneno))
            {
                response = await _bookService.GetBookingDetailByUserAsync(username, phoneno);
            }
            return Ok(response);
        }
    }
}
