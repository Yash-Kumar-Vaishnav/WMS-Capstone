using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Application.DTOs.AuditLog;
using WMS.Application.Interfaces;

namespace WMS.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repository;

    public AuditLogService(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
    {
        var logs = await _repository.GetAllAsync();
        return logs.Select(l => new AuditLogDto
        {
            AuditId = l.AuditId,
            EntityName = l.EntityName,
            RecordId = l.RecordId,
            Action = l.Action,
            CreatedBy = l.CreatedBy,
            CreatedOn = l.CreatedOn
        });
    }
}
