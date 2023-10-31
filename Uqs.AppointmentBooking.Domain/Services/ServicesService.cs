

using Microsoft.EntityFrameworkCore;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Services
{
    public class ServicesService
    {
        private readonly ApplicationContext _context;

        public ServicesService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetActivesServices() => await _context.Services!.Where(x => x.IsActive).ToArrayAsync();
    }
}
