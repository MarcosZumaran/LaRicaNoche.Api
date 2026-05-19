namespace HotelGenericoApi.Models.Exceptions;

public class ExternalServiceException : Exception
{
    public string ErrorCode { get; }

    public ExternalServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = "EXTERNAL_SERVICE_ERROR";
    }

    public ExternalServiceException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
