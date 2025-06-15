namespace OrderManagementSystem.Application.DTOs;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class APIResponse<T>
{
    public int StatusCode { get; set; }
    public List<T>? Data { get; set; }
}