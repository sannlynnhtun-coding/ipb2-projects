using IPB2.OnlineBusSystem.WindowFormApp.Featues.Admin;
using IPB2.OnlineBusSystem.WindowFormApp.Featues.User;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineBusSystem.Database.AppDbContextModels;

namespace IPB2.OnlineBusSystem.WindowFormApp
{
    public partial class MainMaster : Form
    {
        private readonly AppDbContext _db = new AppDbContext();
        private readonly IBusService _busService;
        private readonly IRouteService _routeService;
        private readonly IScheduleService _scheduleService;
        private readonly IBookingService _bookingService;

        public MainMaster()
        {
            InitializeComponent();
            _busService = new BusService(_db);
            _routeService = new RouteService(_db);
            _scheduleService = new ScheduleService(_db);
            _bookingService = new BookingService(_db);
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Admin adminForm = new Admin(_busService, _routeService, _scheduleService, _bookingService);
            adminForm.Show();
            this.Hide();
        }
        private void btnUser_Click(object sender, EventArgs e)
        {
            User userForm = new User(_routeService, _bookingService);
            userForm.Show();
            this.Hide();
        }
    }
}
