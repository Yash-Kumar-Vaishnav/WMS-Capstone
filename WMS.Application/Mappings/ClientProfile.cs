using AutoMapper;
using WMS.Application.DTOs.Client;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();

        CreateMap<CreateClientDto, Client>();

        CreateMap<UpdateClientDto, Client>();
    }
}