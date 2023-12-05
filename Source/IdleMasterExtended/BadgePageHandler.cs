using HtmlAgilityPack;
using IdleMasterExtended.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdleMasterExtended
{
    static internal class BadgePageHandler
    {
        internal static int RetryCount = 0;
        internal static int ReloadCount = 0;
        
        internal const int MaxRetryCount = 18;

        internal static List<Badge> AllBadges { get; set; }

        internal static IEnumerable<Badge> CanIdleBadges
        {
            get
            {
                return AllBadges.Where(badge => badge.RemainingCard != 0)
                                .Where(badge => !Settings.Default.IdleOnlyPlayed || badge.HoursPlayed > 0.0);
            }
        }

        internal static int CardsRemaining { get { return CanIdleBadges.Sum(b => b.RemainingCard); } }

        internal static int GamesRemaining { get { return CanIdleBadges.Count(); } }

        internal static void SortBadges(string method)
        {
            switch (method)
            {
                case "mostcards":
                    AllBadges = AllBadges.OrderByDescending(b => b.RemainingCard).ToList();
                    break;
                case "leastcards":
                    AllBadges = AllBadges.OrderBy(b => b.RemainingCard).ToList();
                    break;
                default:
                    return;
            }
        }

        internal static async Task LoadBadgesAsync()
        {
            if (Settings.Default.IdlingModeWhitelist)
            {
                AllBadges.Clear();

                foreach (var whitelistID in Settings.Default.whitelist)
                {
                    int applicationID;
                    if (int.TryParse(whitelistID, out applicationID)
                        && !AllBadges.Any(badge => badge.AppId.Equals(applicationID)))
                    {
                        AllBadges.Add(new Badge(whitelistID, "App ID: " + whitelistID, "-1", "0"));
                    }
                }
            }
            else
            {
                HtmlDocument htmlDocument;
                int totalBadgePages = 1;

                for (var currentBadgePage = 1; currentBadgePage <= totalBadgePages; currentBadgePage++)
                {
                    if (totalBadgePages == 1)
                    {
                        htmlDocument = await GetBadgePageAsync(currentBadgePage);
                        totalBadgePages = ExtractTotalBadgePages(htmlDocument);
                    }

                    //lblDrops.Text = string.Format(localization.strings.reading_badge_page + " {0}/{1}, " + localization.strings.please_wait, currentBadgePage, totalBadgePages);
                    htmlDocument = await GetBadgePageAsync(currentBadgePage);
                    ProcessBadgesOnPage(htmlDocument);
                }
            }
        }

        private static async Task<HtmlDocument> GetBadgePageAsync(int pageNumber)
        {
            var document = new HtmlDocument();
            var profileLink = Settings.Default.myProfileURL + "/badges";
            var pageURL = string.Format("{0}/?p={1}", profileLink, pageNumber);
            var response = await CookieClient.GetHttpAsync(pageURL);
            CheckIfResponseIsNullWithRetryCount(response);
            document.LoadHtml(response);

            return document;
        }

        private static void CheckIfResponseIsNullWithRetryCount(string response)
        {
            // Response should be empty. User should be unauthorised.
            if (string.IsNullOrEmpty(response))
            {
                RetryCount++;
                throw new Exception("Response is null or empty. Added (+1) to RetryCount");
            }
        }

        private static int ExtractTotalBadgePages(HtmlDocument document)
        {
            // If user is authenticated, check page count. If user is not authenticated, pages are different.
            var pages = new List<string>() { "?p=1" };
            var pageNodes = document.DocumentNode.SelectNodes("//a[@class=\"pagelink\"]");
            if (pageNodes != null)
            {
                pages.AddRange(pageNodes.Select(p => p.Attributes["href"].Value).Distinct());
                pages = pages.Distinct().ToList();
            }

            string lastpage = pages.Last().ToString().Replace("?p=", "");
            int pagesCount = Convert.ToInt32(lastpage);
            return pagesCount;
        }

        private static void ProcessBadgesOnPage(HtmlDocument document)
        {
            foreach (var badge in document.DocumentNode.SelectNodes("//div[@class=\"badge_row is_link\"]"))
            {
                var appIdNode = badge.SelectSingleNode(".//a[@class=\"badge_row_overlay\"]").Attributes["href"].Value;
                var appid = Regex.Match(appIdNode, @"gamecards/(\d+)/").Groups[1].Value;

                if (string.IsNullOrWhiteSpace(appid) || Settings.Default.blacklist.Contains(appid) || appid == "368020" || appid == "335590" || appIdNode.Contains("border=1"))
                {
                    continue;
                }

                var hoursNode = badge.SelectSingleNode(".//div[@class=\"badge_title_stats_playtime\"]");
                var hours = hoursNode == null ? string.Empty : Regex.Match(hoursNode.InnerText, @"[0-9\.,]+").Value;

                var nameNode = badge.SelectSingleNode(".//div[@class=\"badge_title\"]");
                var name = WebUtility.HtmlDecode(nameNode.FirstChild.InnerText).Trim();

                var cardNode = badge.SelectSingleNode(".//span[@class=\"progress_info_bold\"]");
                var cards = cardNode == null ? string.Empty : Regex.Match(cardNode.InnerText, @"[0-9]+").Value;

                var badgeInMemory = AllBadges.FirstOrDefault(b => b.StringId == appid);

                if (badgeInMemory != null)
                {
                    badgeInMemory.UpdateStats(cards, hours);
                }
                else
                {
                    AllBadges.Add(new Badge(appid, name, cards, hours));
                }
            }
        }
    }
}
