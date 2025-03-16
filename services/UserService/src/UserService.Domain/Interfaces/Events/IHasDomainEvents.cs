namespace UserService.Domain.Interfaces.Events;

public interface IHasDomainEvents
{
    IReadOnlyCollection<object> DomainEvents { get; }

    void ClearDomainEvents();
}
