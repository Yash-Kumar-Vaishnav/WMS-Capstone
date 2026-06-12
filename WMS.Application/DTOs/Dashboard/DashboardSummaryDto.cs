namespace WMS.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalEmployees       { get; set; }
    public int TodayAttendanceCount { get; set; }
    public int PendingLeaveRequests { get; set; }
    public int ActiveProjects       { get; set; }
    public int TotalDepartments     { get; set; }
}
