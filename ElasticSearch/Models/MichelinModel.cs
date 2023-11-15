/*
{
    "name": "Kilian Stuba",
    "year": "2019",
    "pin":{"location":{"lat": "47.34858","lon": "10.17114"}}, 
    "city": "Kleinwalsertal",
    "region": "Austria",
    "zipCode": "87568",
    "cuisine": "Creative",
    "price": "$$$$$",
    "url": "https://guide.michelin.com/at/en/vorarlberg/kleinwalsertal/restaurant/kilian-stuba",
    "star": "1"
}
*/
using Nest;

namespace ElasticSearch.Models
{
    public class Pin
    {
        public string lat {get; set; } = default!;
        public string lon {get; set; } = default!;
        
    }
    public class MichelinModel
    {
        public string name {get; set;} = default!;
        public string year {get; set;} = default!;
        public GeoLocation pin {get; set;} = default!;
        public string city {get; set;} = default!;
        public string region {get; set;} = default!;
        public string zipCode {get; set;} = default!;
        public string cuisine {get; set;} = default!;
        public string price {get; set;} = default!;
        public string url {get; set;} = default!;
        public string star {get; set;} = default!;

    }
}