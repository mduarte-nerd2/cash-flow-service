using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlowService.Core.DomainEntities;

public abstract class EntityBase
{
    public EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
     
    }

    public Guid Id { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public DateTimeOffset? CreatedAt { get; private set; }
    
    public DateTimeOffset? SetNewUpdatedAt()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        return UpdatedAt;
    }
}