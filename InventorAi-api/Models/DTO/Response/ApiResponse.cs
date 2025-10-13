namespace InventorAi_api.Models
{
    public class ApiResponse
    {
        public class ResponseData<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
            public ApiError Error { get; set; }
        }

       
    }
}
