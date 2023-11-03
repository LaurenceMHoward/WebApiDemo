using System.Net;

namespace IntegrationTests.Reference;

public class TestResult<T> where T : class
{
    public T? Content { get; set; } = null!;
    public string ExceptionMessage { get; set; } = null!;
    public HttpStatusCode HttpResponseStatus { get; set; }
}