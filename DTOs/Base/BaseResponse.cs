namespace LaRicaNoche.Api.DTOs.Base;

public class BaseResponse<T>
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}
