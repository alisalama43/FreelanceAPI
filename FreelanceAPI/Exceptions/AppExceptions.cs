namespace FreelanceMarketplace.API.Common.Exceptions
{
    /// <summary>
    /// Base type for all handled application exceptions. Carries an HTTP status code
    /// so the global exception middleware can translate it directly into a response.
    /// </summary>
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }

        protected AppException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    /// <summary>404 - the requested resource does not exist.</summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
    }

    /// <summary>400 - the request is malformed or violates a business rule.</summary>
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, StatusCodes.Status400BadRequest) { }
    }

    /// <summary>403 - the authenticated user is not allowed to perform this action.</summary>
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message, StatusCodes.Status403Forbidden) { }
    }

    /// <summary>409 - the request conflicts with the current state of the resource.</summary>
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message, StatusCodes.Status409Conflict) { }
    }

    /// <summary>401 - authentication failed (e.g. bad credentials).</summary>
    public class UnauthorizedAppException : AppException
    {
        public UnauthorizedAppException(string message) : base(message, StatusCodes.Status401Unauthorized) { }
    }
}
