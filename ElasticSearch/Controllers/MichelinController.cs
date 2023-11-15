using ElasticSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearch.Controllers
{
    public class MichelinController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MichelinController(IElasticClient elasticClient, IWebHostEnvironment hostingEnvironment)
        {
            _elasticClient = elasticClient;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Index(string keyword)
        {
            var michelinList = new List<MichelinModel>();
            if (!string.IsNullOrEmpty(keyword))
            {
                michelinList = GetSearch(keyword).ToList();
            }

            return View(michelinList.AsEnumerable());
        }

        
        public IList<MichelinModel> GetSearch(string keyword)
        {
            var result = _elasticClient.SearchAsync<MichelinModel>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + keyword + '*')
                    )).Size(5000));

            var finalResult = result;
            var finalContent = finalResult.Result.Documents.ToList();
            return finalContent;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MichelinModel model)
        {
            // try
            // {
            //     var michelin = new MichelinModel()
            //     {
            //         Id = 1,
            //         Title = model.Title,
            //         Link = model.Link,
            //         Author = model.Author,
            //         AuthorLink = model.AuthorLink,
            //         PublishedDate = DateTime.Now
            //     };

            //     await _elasticClient.IndexDocumentAsync(michelin);
            //     model = new MichelinModel();
            // }
            // catch (Exception ex)
            // {
            // }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MichelinModel model)
        {
            // try
            // {
            //     var michelin = new MichelinModel()
            //     {
            //         Id = 1,
            //         Title = model.Title,
            //         Link = model.Link,
            //         Author = model.Author,
            //         AuthorLink = model.AuthorLink,
            //         PublishedDate = DateTime.Now
            //     };

            //     await _elasticClient.DeleteAsync<MichelinModel>(michelin);
            //     model = new MichelinModel();
            // }
            // catch (Exception ex)
            // {
            // }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import()
        {
            // try
            // {
            //     var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

            //     var fullPath =
            //         Path.Combine(rootPath,
            //             "2019-michelin-resturants.json"); //combine the root path with that of our json file inside mydata directory

            //     var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

            //     if (jsonData != null)
            //     {
            //         _elasticClient.BulkAsync(Func<BulkDescriptor, IBulkRequest> selector, CancellationToken ct = default(CancellationToken));

            //     }
            // }
            // catch (Exception ex)
            // {
            // }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkImport()
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

                //combine the root path with that of our json file inside mydata directory
                var fullPath = Path.Combine(rootPath,"michelin.json");

                //read all the content inside the file
                var jsonData = System.IO.File.ReadAllText(fullPath);
                var michelinList = JsonConvert.DeserializeObject<List<MichelinModel>>(jsonData);

                if (michelinList != null)
                {
                    //var bulkIndexResponse _elasticClient.Bulk(b => b.Index("michelin").IndexMany(michelinList) );

                    // synchronous method that returns an IBulkResponse
                    var indexManyResponse = _elasticClient.IndexMany(michelinList);
                    if (indexManyResponse.Errors)
                    {
                        // the response can be inspected for errors
                        foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                        {
                            // if there are errors, they can be enumerated and inspected
                            Console.WriteLine("Failed to index document {0}: {1}",
                            itemWithError.Id, itemWithError.Error);
                        }
                    }
                    // alternatively, documents can be indexed asynchronously
                    //var indexManyAsyncResponse = _elasticClient.IndexManyAsync(michelinList);

                }
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Index");

        }

    }

}
