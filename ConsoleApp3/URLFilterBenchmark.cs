using BenchmarkDotNet.Attributes;

namespace Che.Coxshall
{
    [MemoryDiagnoser]
    public class URLFilterBenchmark
    {
        static List<string> urlsToCheck = [
            "http://example.com/page1",
            "http://example.com/page2",
            "http://example.com/sub/page3",
            "http://subdomain.example.com/page4",
            "http://anotherdomain.com/page1",
            "http://blocked.com/page2",
            "http://subdomain.blocked.com/page1",
            "http://example.com/sub/page5",
            "http://example.com/page6",
            "http://subdomain.example.com/sub/page7",
        ];

        static List<string> blockedUrls = [
            "http://blocked.com",
            "http://example.com/sub",
            "http://anotherdomain.com",
        ];

        [Benchmark]
        public string[] UriParse() => UrlFilter.FilterBlockedUrls(urlsToCheck, blockedUrls).ToArray();

        public static void Run()
        {
            var result = UrlFilter.FilterBlockedUrls(urlsToCheck, blockedUrls);

            Console.WriteLine("Filtered URLs:");
            foreach (var url in result)
                Console.WriteLine(url);
        }
    }
}