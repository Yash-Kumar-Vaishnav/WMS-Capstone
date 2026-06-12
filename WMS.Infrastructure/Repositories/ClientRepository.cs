using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class ClientRepository
    : GenericRepository<Client>,
      IClientRepository
{
    public ClientRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Client>> GetActiveClientsAsync()
    {
        return await _context.Clients
            .Where(x => x.Status)
            .ToListAsync();
    }
}