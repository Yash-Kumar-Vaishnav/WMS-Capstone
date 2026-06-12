using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IClientRepository : IGenericRepository<Client>
{
    Task<IEnumerable<Client>> GetActiveClientsAsync();
}