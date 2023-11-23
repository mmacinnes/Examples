//using System.Text.Json;
//using System.Text.Encodings.Web;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using TodoClient.Models;
using System.Diagnostics;

namespace TodoClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ToDoList()
    {
            List<TodoItem> todoList = new List<TodoItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7193/api/TodoItems"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    todoList = JsonConvert.DeserializeObject<List<TodoItem>>(apiResponse);
                }
            }
            return View(todoList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
