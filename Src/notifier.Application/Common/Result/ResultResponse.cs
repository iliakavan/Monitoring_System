namespace notifier.Application.Common.Result;



public class ResultResponse
{
    public bool Success { get; set; }

    public string? Message { get; set; }

}

public class ResultResponse<T> where T : class
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public T Value { get; set; } = null!;

}