using Election2023.Domain.Common;

namespace Election2023.DataStore;

public class Audit : IEntity<int>
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Table { get; set; } = string.Empty;

    public string PrimaryKey { get; set;} = string.Empty;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? AffectedColumns { get; set; }

    public DateTime TimeStamp { get; set; }
}

