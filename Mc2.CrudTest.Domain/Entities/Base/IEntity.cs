
namespace Mc2.CrudTest.Domain.Entities.Base;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}