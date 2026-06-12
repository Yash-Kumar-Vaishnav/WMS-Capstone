using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UserLogin user, string roleName, string? empId = null);
}