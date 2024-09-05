using Bogus;
using Che.Coxshall;
using System;

namespace TestProject1
{
    public class UrlFilterTest
    {
        [Theory]
        [InlineData(100, 10000)]
        [InlineData(1000, 1000000)]
        [InlineData(10000, 100000000)]
        [InlineData(100000, 10000000000)]
        public void Test1(int blockedCount, long urlCount)
        {
            var f = new Faker();
            var blockedList = GenerateBlockedUrls(blockedCount).ToArray();
            List<string> generatedBlocked = [];

            var testList = Enumerable.Range(0, blockedCount).Select(x =>
            {
                if (f.Random.Bool(0.2f))
                {
                    var blockedUrl = GenerateBlockedUrl(blockedList);
                    generatedBlocked.Add(blockedUrl);
                    return blockedUrl;
                }
                return f.Internet.Url();
            });

            var results = UrlFilter.FilterBlockedUrls(blockedList, testList).ToArray();

            Assert.False(results.Any(x => generatedBlocked.Contains(x)));
        }

        private string GenerateBlockedUrl(string[] urls)
        {
            var f = new Faker();
            var url = new UriBuilder(f.PickRandom(urls));
            if (f.Random.Bool())
                url.Host = $"{f.Internet.DomainWord()}.{url.Host}";
            if (f.Random.Bool())
                url.Path += f.Internet.UrlRootedPath();
            return url.ToString();
        }

        private IEnumerable<string> GenerateBlockedUrls(int count)
        {
            var f = new Faker();
            for (int i = 0; i < count; i++)
            {
                var builder = new UriBuilder();
                builder.Scheme = "https";
                builder.Host = f.Random.Bool(0.2f) ? $"{f.Internet.DomainWord()}.{f.Internet.DomainName()}" : f.Internet.DomainName();
                if (f.Random.Bool())
                    builder.Path = f.Internet.UrlRootedPath();

                yield return builder.ToString();
            }
        }
    }
}