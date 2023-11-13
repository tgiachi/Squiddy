using Squiddy.Core.Interfaces.Events;

namespace Squiddy.Core.Data.Events;

public abstract class AbstractBaseEvent : IBaseEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
