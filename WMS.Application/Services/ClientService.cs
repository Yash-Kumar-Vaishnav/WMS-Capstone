using AutoMapper;
using WMS.Application.DTOs.Client;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientService(
        IClientRepository clientRepository,
        IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ClientDto>>(clients);
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client == null)
            return null;

        return _mapper.Map<ClientDto>(client);
    }

    public async Task<int> CreateAsync(CreateClientDto dto)
    {
        var client = _mapper.Map<Client>(dto);

        await _clientRepository.AddAsync(client);
        await _clientRepository.SaveChangesAsync();

        return client.ClientId;
    }

    public async Task<bool> UpdateAsync(UpdateClientDto dto)
    {
        var client = await _clientRepository.GetByIdAsync(dto.ClientId);

        if (client == null)
            return false;

        _mapper.Map(dto, client);

        _clientRepository.Update(client);
        await _clientRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client == null)
            return false;

        _clientRepository.Delete(client);
        await _clientRepository.SaveChangesAsync();

        return true;
    }
}