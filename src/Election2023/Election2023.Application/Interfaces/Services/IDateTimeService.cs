namespace Election2023.Application.Interfaces.Services;

public interface IDateTimeService
{
	DateTime UtcNow { get; }

	DateTime Now { get; }
}

