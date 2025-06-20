namespace OrderServices.Helpers
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string message, T? data = default, List<string>? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
            => new ApiResponse<T>(true, message, data);

        public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null)
            => new ApiResponse<T>(false, message, default, errors);
    }
}
