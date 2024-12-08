namespace Chat.API.Domain.BaseEntities;

public class BaseAuditableEntity : BaseEntity
{
    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }
}