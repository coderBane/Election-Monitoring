namespace Election2023.Shared.Wrapper;

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

public interface IResult
{
    IEnumerable<string> Messages { get; set; }

    bool Success { get; set; }
}