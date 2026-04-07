using IPB2.OnlineBusSystem.Database.AppDbContextModels;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPB2.OnlineBusSystem.Domain
{
    public static class FeatureManager
    {
        public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
        {
            // Features
            builder.Services.AddScoped<IBusService, BusService>();
            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<IBookingService, BookingService>();


            return builder;
        }

        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }
    }
}
