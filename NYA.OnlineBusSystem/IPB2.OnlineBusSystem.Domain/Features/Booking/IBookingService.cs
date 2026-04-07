using IPB2.OnlineBusSystem.Domain.Common;

namespace IPB2.OnlineBusSystem.Domain.Features.Booking
{
    public interface IBookingService
    {
        Task<ServiceResponse> CreateAsync(BookRequest request);
        Task<SearchBusResponse> SearchBus(SearchBusRequest request);
        Task<BookingDetailResponse> GetBookingDetailByUserAsync(string username, string phoneno);
        Task<BookingDetailResponse> GetBookingDetailAsync(string? search);
        Task<ServiceResponse> CancelAsync(string bookingId);
    }
}