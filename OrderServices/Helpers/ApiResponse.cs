namespace OrderServices.Helpers
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string message, T? data = default, Dictionary<string, List<string>>? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        public static ApiResponse<T> FailureResponse(string message, Dictionary<string, List<string>>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = errors
            };
        }
    }
}
