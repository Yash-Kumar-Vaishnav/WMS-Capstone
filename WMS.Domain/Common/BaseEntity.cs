namespace WMS.Domain.Common;

public abstract class BaseEntity
{
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime? UpdatedOn { get; set; }
}