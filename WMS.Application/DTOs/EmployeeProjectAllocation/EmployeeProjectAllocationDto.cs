namespace WMS.Application.DTOs.EmployeeProjectAllocation;

public class EmployeeProjectAllocationDto
{
    public int AllocationId { get; set; }

    public int EmpId { get; set; }

    public int ProjectId { get; set; }

    public DateTime AssignedOn { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public bool Status { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}