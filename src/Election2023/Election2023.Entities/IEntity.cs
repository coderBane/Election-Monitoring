#region global_annotations
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
#endregion

using System.Text.Json;

namespace Election2023.Entities;

public interface IEntity
{
    int Id { get; }
}


public abstract class Entity : IEntity
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; private set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
