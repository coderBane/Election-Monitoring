using System.Text.Json;

namespace Election2023.Domain.Common;

public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
{
#nullable disable
    [Column(Order = 0)]
    public TId Id { get; protected set; }
#nullable enable

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public string LastModifiedBy { get; set; } = string.Empty;

    public DateTime LastModifiedOn { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
    }

public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
{

}

public interface IAuditableEntity : IEntity
{
    string CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    string LastModifiedBy { get; set; }

    DateTime LastModifiedOn { get; set; }
}

