using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Election2023.DataStore;

public enum AuditType
{
    None, Create, Update, Delete
}

public class AuditEntry
{
    public AuditEntry(EntityEntry entityEntry)
    {
        EntityEntry = entityEntry;
    }

    public EntityEntry EntityEntry { get; }

    /// <summary>User persisting changes</summary>
    public string UserId { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public Dictionary<string, object> KeyValues { get; } = new();
    public Dictionary<string, object> OldValues { get; } = new();
    public Dictionary<string, object> NewValues { get; } = new();
    public AuditType AuditType { get; set; }
    /// <summary>Properties with database generated value</summary>
    public List<PropertyEntry> TemporaryProperties { get; } = new();
    public List<string> ChangedColumns { get; } = new();
    public bool HasTemporaryproperties => TemporaryProperties.Any();

    public Audit ToAudit() => new()
    {
        UserId = UserId,
        Table = TableName,
        Type = AuditType.ToString(),
        TimeStamp = DateTime.UtcNow,
        PrimaryKey = JsonSerializer.Serialize(KeyValues),
        OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
        NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
        AffectedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns)
    };
}
