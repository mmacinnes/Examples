using ElasticSearch.Models;
using Nest;

namespace ElasticSearch
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];

            var settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
                .PrettyJson()
                .CertificateFingerprint("1fc2f85ac8be66eb1325f3cb9266d244f0b52fe6e2b43706ccdc328e2d618f4d")
                .BasicAuthentication("elastic", "I3QqWwZ6Thk_fm5K5E8V")
                .DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, index);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<ArticleModel>(m => m
                    .Ignore(p => p.Link)
                    .Ignore(p => p.AuthorLink)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<ArticleModel>(x => x.AutoMap())
            );
        }
    }
}
