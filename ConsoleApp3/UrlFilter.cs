namespace Che.Coxshall
{
    public static class UrlFilter
    {
        /// <summary>
        /// The <paramref name="urlsToCheck"/> paramater contains URLs which should be checked if they are blocked.
        /// The <paramref name="blockedUrls"/> parameter contains URLs which are blocked.
        /// </summary>
        /// <remarks>
        /// If some path is blocked, it also blocks all its sub-paths.
        /// If some domain is blocked, it also blocks all its subdomains.
        /// </remarks>
        /// <param name="urlsToCheck">URLs which should be checked if they are blocked.</param>
        /// <param name="blockedUrls"> URLs which are blocked.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of urls that are not blocked.</returns>
        public static IEnumerable<string> FilterBlockedUrls(IEnumerable<string> urlsToCheck, IEnumerable<string> blockedUrls)
        {
            HashSet<Uri> blockedSet = [];

            foreach (var blockedUrl in blockedUrls)
                if (Uri.TryCreate(blockedUrl, UriKind.Absolute, out var uri) && !blockedSet.Contains(uri))
                    blockedSet.Add(uri);

            foreach (var url in urlsToCheck.Where(x => !IsBlocked(blockedSet, new Uri(x))))
                yield return url;
        }

        private static bool IsBlocked(HashSet<Uri> blockedSet, Uri uriToCheck)
        {
            foreach (var blockedUri in blockedSet)
                if (uriToCheck.Host.EndsWith(blockedUri.Host, StringComparison.OrdinalIgnoreCase) && uriToCheck.AbsolutePath.StartsWith(blockedUri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}