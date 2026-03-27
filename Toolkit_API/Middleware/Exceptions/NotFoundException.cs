namespace Toolkit_API.Middleware.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) 
            : base(message,404) { }
    }
}
