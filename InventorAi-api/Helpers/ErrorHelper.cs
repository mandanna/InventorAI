using InventorAi_api.Models;
using static InventorAi_api.Enums.ErrorCodes;

namespace InventorAi_api.Helpers
{
    public static class ErrorHelper
    {
        public static ApiError FromErrorCode(ErrorCode code, string? customMessage = null)
        {
            var defaultMessage = code switch
            {
                ErrorCode.InvalidInput => "Invalid input provided.",
                ErrorCode.NotFound => "Requested record not found.",
                ErrorCode.Unauthorized => "User is not authorized.",
                ErrorCode.InActive =>"InActive",
                ErrorCode.Expired => "Expired",
                _ => "An unexpected error occurred."
            };

            return new ApiError
            {
                Code = code.ToString().ToUpper(),
                Message = customMessage ?? defaultMessage
            };
        }
    }
}
