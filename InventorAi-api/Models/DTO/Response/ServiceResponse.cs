namespace InventorAi_api.Models
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }
        public string Message { get; set; }
    }
}
