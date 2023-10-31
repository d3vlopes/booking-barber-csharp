using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Uqs.AppointmentBooking.Domain.DomainObjects;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.Report;

namespace Uqs.AppointmentBooking.Domain.Services
{
    public class SlotsService
    {
        private readonly ApplicationContext _context;
        private readonly ApplicationSettings _settings;
        private readonly DateTime _now;
        private readonly TimeSpan _roundingIntervalSpan;

        public SlotsService(ApplicationContext context, INowService nowService, IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
            _now = RoundUpToNearest(nowService.Now);

        }

        public async Task<Slots> GetAvailableSlotsForEmployee(int serviceId, int employeeId)
        {
            var service = await _context.Services!.SingleOrDefaultAsync(x => x.Id == serviceId);

            if (service is null)
            {
                throw new ArgumentException("Record not found", nameof(serviceId));
            }

            var shifts = _context.Shifts!.Where(x => x.EmployeeId == employeeId);

            if (!shifts.Any())
            {
                return new Slots(Array.Empty<DaySlots>());
            }

            return null;
        }

        private DateTime RoundUpToNearest(DateTime dt)
        {
            var ticksInSpan = _roundingIntervalSpan.Ticks;
            return new DateTime((dt.Ticks + ticksInSpan - 1)
                / ticksInSpan * ticksInSpan, dt.Kind);
        }
    }
}
