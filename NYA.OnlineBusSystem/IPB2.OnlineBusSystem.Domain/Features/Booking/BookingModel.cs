namespace IPB2.OnlineBusSystem.Domain.Features.Booking;

public class SearchBusRequest
{
    public DateOnly TravelDate { get; set; }
    public string Origin { get; set; } = null!;
    public string Destination { get; set; } = null!;
}

public class SearchBusResponseModel
{
    public string secheduleId { get; set; } = null!;
    public string BusNo { get; set; } = null!;
    public string BusName { get; set; } = null!;

    public string BusType { get; set; } = null!;
    public string DepartureTime { get; set; } = null!;
    public string ArrivalTime { get; set; } = null!;
    public int AvaliableSeat { get; set; }
    public int Fare { get; set; }
}
public class SearchBusResponse
{
    public List<SearchBusResponseModel> Buss { get; set; } = new();
}
public class Passenger
{
    public int SeatNo { get; set; }
    public string Username { get; set; } = null!;
    public string Phoneno { get; set; } = null!;
}

public class BookRequest
{
    public string ScheduleId { get; set; } = null!;
    public List<Passenger> Passengers { get; set; } = new();
}


public class BookingDetailResponse
{
    public List<BookingDetailModel> bookings { get; set; } = new();
}
public class BookingDetailModel
{
    public string Id { get; set; }
    public DateTime TravelDate { get; set; }
    public string BookedArrivalTime { get; set; }
    public string BookedDepartureTime { get; set; }
    public decimal BookedFare { get; set; }
    public string Username { get; set; }
    public string Phoneno { get; set; }
    public string SeatNo { get; set; }
    public string BookedBusNo { get; set; }
    public string BookedBusName { get; set; }
    public string BookedBusType { get; set; }
    public string BookedOrigin { get; set; }
    public string BookedDestination { get; set; }
}
