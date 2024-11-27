namespace DarazApp.Responses
{

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();


        public ApiResponse(T data, string message = null)
        {
            Success = true;

            Data = data;

            Message = message;

            Errors = null; // No errors on success
        }

        // Constructor for error responses
        public ApiResponse(string message, List<string> errors = null)
        {
            Success = false;

            Data = default; // No data on error

            Message = message;

            Errors = errors;
        }

    }
}
