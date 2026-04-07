using Dapper;
using IPB2.OnlineBusSystem.Database.AppDbContextModels;
using IPB2.OnlineBusSystem.Domain.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IPB2.OnlineBusSystem.Domain.Features.Booking
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _db;
        public BookingService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<SearchBusResponse> SearchBus(SearchBusRequest request)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.GetConnection()))
            {
                db.Open();

                var sql = $@"SELECT s.id as secheduleId,b.BusNo,b.BusName,b.BusType,s.DepartureTime,s.ArrivalTime,s.AvaliableSeat,s.Fare
                        FROM [dbo].[Tbl_Schedule] s
                        INNER JOIN [dbo].[Tbl_Route] r ON s.RouteId = r.ID
                        INNER JOIN [dbo].[Tbl_BusDetail] b ON s.BusId = b.ID
                        WHERE 
                            s.IsDelete = 0 
                            AND r.IsDelete = 0
                            AND b.IsDelete = 0
                            AND s.AvaliableSeat > 0
                            AND s.Date = '{request.TravelDate}'
                            AND r.Origin = '{request.Origin}'
                            AND r.Destination = '{request.Destination}'";

                List<SearchBusResponseModel> results = db.Query<SearchBusResponseModel>(sql).ToList();
                return new SearchBusResponse { Buss = results };
            }
        }

        public async Task<ServiceResponse> CreateAsync(BookRequest request)
        {

            var schedule = await _db.TblSchedules.FirstOrDefaultAsync(x => x.Id == request.ScheduleId && !x.IsDelete);
            if (schedule == null)
                return new ServiceResponse { Status = ResponseType.NotFound, Message = "Schedule not found." };

            int totalSeatsToBook = request.Passengers.Count;
            int maxCapacity = schedule.AvaliableSeat + schedule.BookSeat;

            if (schedule.AvaliableSeat < totalSeatsToBook)
                return new ServiceResponse { Status = ResponseType.None, Message = "Not enough seats available." };

            var requestedSeats = request.Passengers.Select(x => x.SeatNo).ToList();
            if (requestedSeats.Count != requestedSeats.Distinct().Count())
            {
                return new ServiceResponse { Status = ResponseType.None, Message = "Duplicate seat numbers found in your request." };
            }

            if (requestedSeats.Any(s => s > maxCapacity))
                return new ServiceResponse { Status = ResponseType.None, Message = "One or more Seat numbers are invalid for this schedule." };


            var alreadyBooked = await _db.TblBooks
                .Where(x => x.ScheduleId == request.ScheduleId && !x.IsDelete && requestedSeats.Contains(x.Seatno))
                .Select(x => x.Seatno)
                .ToListAsync();

            if (alreadyBooked.Any())
                return new ServiceResponse { Status = ResponseType.AlreadyExists, Message = $"Seats already taken: {string.Join(", ", alreadyBooked)}" };

            // 4. Bulk Add Bookings
            var bookings = request.Passengers.Select(p => new TblBook
            {
                Id = Guid.NewGuid().ToString(),
                ScheduleId = request.ScheduleId,
                Seatno = p.SeatNo,
                Username = p.Username,
                Phoneno = p.Phoneno,
                IsDelete = false
            });

            _db.TblBooks.AddRange(bookings);

            schedule.AvaliableSeat -= totalSeatsToBook;
            schedule.BookSeat += totalSeatsToBook;

            return await _db.SaveChangesAsync() > 0
                ? new ServiceResponse { Status = ResponseType.Success, Message = "Booking created successfully." }
                : new ServiceResponse { Status = ResponseType.None, Message = "Failed to save booking." };
        }
        public async Task<BookingDetailResponse> GetBookingDetailByUserAsync(string username, string phoneno)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.GetConnection()))
            {
                db.Open();

                var sql = $@"select book.Id, s.Date as TravelDate,book.Username,
                            book.Phoneno,book.Seatno as SeatNo,b.BusNo as BookedBusNo,b.BusName as BookedBusName,
							b.BusType as BookedBusType,s.ArrivalTime as BookedArrivalTime,
                            s.DepartureTime as BookedDepartureTime,s.Fare as BookedFare,r.Origin as BookedOrigin,r.Destination as BookedDestination
							FROM [dbo].[Tbl_Schedule] s
							INNER JOIN [dbo].[Tbl_Route] r ON s.RouteId = r.ID
							INNER JOIN [dbo].[Tbl_BusDetail] b ON s.BusId = b.ID
							INNER JOIN [dbo].[Tbl_Book] book ON s.Id = book.ScheduleId
                            WHERE book.Username = @Username AND book.Phoneno = @Phoneno AND book.IsDelete = 0
                        ";

                List<BookingDetailModel> results = db.Query<BookingDetailModel>(sql, new { Username = username, Phoneno = phoneno }).ToList();
                return new BookingDetailResponse { bookings = results };
            }
        }

        public async Task<BookingDetailResponse> GetBookingDetailAsync(string? search)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.GetConnection()))
            {
                db.Open();
                var sql = $@"select book.Id, s.Date as TravelDate,book.Username,
                            book.Phoneno,book.Seatno as SeatNo,b.BusNo as BookedBusNo,b.BusName as BookedBusName,
							b.BusType as BookedBusType,s.ArrivalTime as BookedArrivalTime,
                            s.DepartureTime as BookedDepartureTime,s.Fare as BookedFare,r.Origin as BookedOrigin,r.Destination as BookedDestination
							FROM [dbo].[Tbl_Schedule] s
							INNER JOIN [dbo].[Tbl_Route] r ON s.RouteId = r.ID
							INNER JOIN [dbo].[Tbl_BusDetail] b ON s.BusId = b.ID
							INNER JOIN [dbo].[Tbl_Book] book ON s.Id = book.ScheduleId
                            WHERE book.IsDelete = 0 ";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += $@" AND (b.BusNo LIKE '%{search}%' 
                      OR b.BusName LIKE '%{search}%'  
                      OR book.Username LIKE '%{search}%' )";
                }

                List<BookingDetailModel> results = db.Query<BookingDetailModel>(sql).ToList();
                return new BookingDetailResponse { bookings = results };
            }
        }
        public async Task<ServiceResponse> CancelAsync(string bookingId)
        {
            var booking = await _db.TblBooks.FirstOrDefaultAsync(x => x.Id == bookingId && !x.IsDelete);
            if (booking == null)
                return new ServiceResponse { Status = ResponseType.NotFound, Message = "Booking not found." };

            var schedule = await _db.TblSchedules.FirstOrDefaultAsync(x => x.Id == booking.ScheduleId);
            if (schedule != null)
            {
                schedule.AvaliableSeat += 1;
                schedule.BookSeat -= 1;
            }

            booking.IsDelete = true;
            return await _db.SaveChangesAsync() > 0
                ? new ServiceResponse { Status = ResponseType.Success, Message = "Booking cancelled successfully." }
                : new ServiceResponse { Status = ResponseType.None, Message = "Failed to cancel booking." };
        }
    }
}
