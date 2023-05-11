namespace Election2023.Domain.Common;

public abstract class Entity<TId> : IEntity<TId>
{
	#nullable disable
	[Column(Order = 0)]
    public TId Id { get; protected set; }

#nullable enable
}

public interface IEntity<TId> : IEntity
{
	TId Id { get; }
}

public interface IEntity { }

