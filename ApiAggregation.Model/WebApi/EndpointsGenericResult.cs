namespace Model.ApiResponse;

public class EndpointsGenericResult<T>
{
    public bool Success { get; set; }
    
    public T? Data { get; set; }
}