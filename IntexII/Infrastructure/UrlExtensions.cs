namespace IntexII.Infrastructure
{
    public static class UrlExtensions
    {        
        public static string PathAndQuery(this HttpRequest request) =>
            request.QueryString.HasValue ? $"{request.Path}{request.Query}" : request.Path.ToString();

    }
}
