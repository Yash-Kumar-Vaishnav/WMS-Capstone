using WMS.Application.DTOs.Client;

namespace WMS.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllAsync();

    Task<ClientDto?> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateClientDto dto);

    Task<bool> UpdateAsync(UpdateClientDto dto);

    Task<bool> DeleteAsync(int id);
}