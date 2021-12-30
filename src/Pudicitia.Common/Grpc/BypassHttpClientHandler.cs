namespace Pudicitia.Common.Grpc;

public class BypassHttpClientHandler : HttpClientHandler
{
    public BypassHttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = DangerousAcceptAnyServerCertificateValidator;
    }
}
