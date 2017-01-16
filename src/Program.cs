using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using RankingScraper.Model;
using Raven.Client.Document;
using ServiceStack;
using ServiceStack.Text;

namespace RankingScraper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var overviewPageSource = "http://sb.tournamentsoftware.com/ranking/ranking.aspx?rid=185".GetStringFromUrl();

            var doc = new HtmlDocument();
            doc.LoadHtml(overviewPageSource);
            var options = doc.QuerySelectorAll("#cphPage_cphPage_cphPage_dlPublication > option");

            var weekIds = new List<WeekEntry>();

            foreach (var option in options)
            {
                weekIds.Add(new WeekEntry()
                {
                    Id = option.GetAttributeValue("value", null),
                    WeekNumber = int.Parse(option.NextSibling.InnerText.Split('-')[0]),
                });
            }

            var rankings = ScrapeRankingForTeam(79397, weekIds);

            rankings.PrintDump();

            var store = new DocumentStore()
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Smash"
            }.Initialize();

            using (var session = store.OpenSession())
            {
                foreach (var rankingSnapshot in rankings)
                {
                    session.Store(rankingSnapshot);
                }

                session.SaveChanges();
            }

            Console.ReadLine();
        }

        private static List<RankingSnapshot> ScrapeRankingForTeam(int teamId, List<WeekEntry> weeks)
        {
            Console.WriteLine($"Scraping for team {teamId}");
            List<RankingSnapshot> rankings = new List<RankingSnapshot>();

            foreach (var week in weeks)
            {
                var rankingSnapshot = new RankingSnapshot();
                rankingSnapshot.Week = week;

                Console.WriteLine($"Scraping week {week}");
                var html = "http://sb.tournamentsoftware.com/ranking/category.aspx"
                    .AddQueryParam("id", week.Id)
                    .AddQueryParam("category", 2338)
                    .AddQueryParam("C2338FOG", teamId)
                    .GetStringFromUrl();

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var rows = doc.QuerySelectorAll("tr");

                for (int i = 2; i < rows.Count; ++i)
                {
                    var row = rows[i].ChildNodes.Select(cn => cn.InnerText).ToList();

                    if (row.Count < 11) continue; // first, 2nd alnd last has different row.Count... 

                    var playerLink = rows[i].ChildNodes[4].ChildNodes[0].GetAttributeValue("href", string.Empty);

                    var playerIdStartPos = playerLink.LastIndexOf("player=");

                    var ranking = new PlayerRanking()
                    {
                        Id = int.Parse(playerLink.Substring(playerIdStartPos + 7 + 1)), // TODO int.Parse(System.Web.HttpUtility.ParseQueryString(rows[i].ChildNodes[4].ChildNodes[0].GetAttributeValue("href")).Get("player")),
                        FullName = row[4],
                        Points = int.Parse(row[6]),
                        InterclubPoints = int.Parse(row[7]),
                        TotalPoints = int.Parse(row[9]),
                        Level = row[11],
                        TournamentCount = int.Parse(row[10])
                    };

                    rankingSnapshot.PlayerRankings.Add(ranking);
                }

                rankings.Add(rankingSnapshot);
            }

            return rankings;
        }

        ////public static class HttpUtils
        ////{
        ////    public static string GetQueryParam(this string url, string key)
        ////    {
        ////        if (string.IsNullOrEmpty(url)) return null;
        ////        var qsPos = url.IndexOf('?');
        ////        if (qsPos != -1)
        ////        {
        ////            var existingKeyPos = qsPos + 1 == url.IndexOf(key, qsPos, PclExport.Instance.InvariantComparison)
        ////                ? qsPos
        ////                : url.IndexOf("&" + key, qsPos, PclExport.Instance.InvariantComparison);

        ////            if (existingKeyPos != -1)
        ////            {
        ////                var endPos = url.IndexOf('&', existingKeyPos + 1);
        ////                if (endPos == -1)
        ////                    endPos = url.Length;

        ////                url.Substring()

        ////                var newUrl = url.Substring(0, existingKeyPos + key.Length + 1)
        ////                    + "="
        ////                    + val.UrlEncode()
        ////                    + url.Substring(endPos);
        ////                return newUrl;
        ////            }
        ////        }

        ////        return string.Empty;
        ////    }
        ////}
    }
}
