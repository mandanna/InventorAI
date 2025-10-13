namespace InventorAi_api.Models;

public class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Field { get; set; } // optional, useful for validation
}
