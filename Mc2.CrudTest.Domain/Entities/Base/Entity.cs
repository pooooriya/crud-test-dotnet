namespace Mc2.CrudTest.Domain.Entities.Base;
public class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ModifiedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}

public class Entity : Entity<long>
{
}