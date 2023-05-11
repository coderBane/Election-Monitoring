namespace Election2023.Shared.Wrapper;

public class Result : IResult
{
    public Result()
    {
        Messages = Enumerable.Empty<string>();
    }

    public IEnumerable<string> Messages { get ; set; }

    public bool Success { get; set; }

    public static IResult Fail() => new Result { Success = false };

    public static IResult Fail(params string[] messages) => new Result { Success = false, Messages = messages };

    public static IResult Pass() => new Result { Success = true };

    public static IResult Pass(params string[] messages) => new Result { Success = true, Messages = messages };
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; set; }

    public static IResult<T> Pass(T data) => new Result<T> { Data = data, Success = true };

    public static IResult<T> Pass(T data, params string[] messages) 
        => new Result<T> { Success = true, Data = data, Messages = messages };
}

