namespace TagsCloudResult;

public class MyResult<T>(string? error, T value = default)
{
    public bool IsOk => error == null;
    public bool IsErr => error != null;
    
    public T Unwrap()
    {
        return value;
    }
    
    public string? UnwrapErr()
    {
        return error;
    }
    
    public static MyResult<T> Err(string error)
    {
        return new MyResult<T>(error);
    }
    
    public static MyResult<T> Ok(T value)
    {
        return new MyResult<T>(null, value);
    }

    public void ReplaceError(string text)
    {
        error += text;
    }
}

public class MyResult(string? error)
{
    public bool IsOk => error == null;
    public bool IsErr => error != null;
    
    public string? UnwrapErr()
    {
        return error;
    }
    
    public static MyResult Err(string error)
    {
        return new MyResult(error);
    }
    
    public static MyResult Ok()
    {
        return new MyResult(null);
    }
    
    public static MyResult<T> Ok<T>(T value)
    {
        return MyResult<T>.Ok(value);
    }
    
    public static MyResult<T> Try<T>(Func<T> action)
    {
        try
        {
            return MyResult<T>.Ok(action());
        }
        catch (Exception e)
        {
            return MyResult<T>.Err(e.Message);
        }
    }
}