namespace Project2UI.Models;


public class Project2Response
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class Project2Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Result { get; set; }
}