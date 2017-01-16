using Funq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ServiceStack;
using Smash.Jobs;
using Smash.Model;

namespace Smash
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Smash", typeof(Startup).GetAssembly())
        {
        }

        public override void Configure(Container container)
        {
            var store = new DocumentStore()
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Smash",
            }.Initialize();

            IndexCreation.CreateIndexes(typeof(Ranking_Index).GetAssembly(), store);
            
            container.Register(store);

            //var scraper = new RankingScraper(store);
            //scraper.Scrape();
        }
    }
}