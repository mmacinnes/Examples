//using System.Text.Json;
//using System.Text.Encodings.Web;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using TodoClient.Models;
using System.Diagnostics;
using System.Text;

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

    [HttpGet]
    public async Task<IActionResult> ToDoDetail(long id)
    {
        TodoItem todoItem = new TodoItem();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("https://localhost:7193/api/TodoItems/" + id))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todoItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);
                }
                else
                    ViewBag.StatusCode = response.StatusCode;
            }
        }
        return View(todoItem);
    }


    [HttpGet]
    public async Task<IActionResult> ToDoEdit(long id)
    {
        TodoItem todoItem = new TodoItem();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("https://localhost:7193/api/TodoItems/" + id))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todoItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);
                }
                else
                    ViewBag.StatusCode = response.StatusCode;
            }
        }
        return View("ToDoEdit",todoItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToDoEdit(long id, [Bind("Id,Name,IsComplete")] TodoItem todoItem)
    {
    
    using (var httpClient = new HttpClient())
    {
        
        StringContent content = new StringContent(JsonConvert.SerializeObject(todoItem), Encoding.UTF8, "application/json");

        using (var response = await httpClient.PutAsync("https://localhost:7193/api/TodoItems/" + id, content))
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ToDoList");
            }
            else
                ViewBag.StatusCode = response.StatusCode;
            }
        }
        return View("ToDoEdit",todoItem);
    }

    public IActionResult ToDoCreate()
    {
            return View();
    }

    [HttpPost]
    public async Task<IActionResult> ToDoCreate(TodoItem todoItem)
    {
            TodoItem receivedItem = new TodoItem();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(todoItem), Encoding.UTF8, "application/json");
 
                using (var response = await httpClient.PostAsync("https://localhost:7193/api/TodoItems", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);

                    return RedirectToAction("ToDoList");
                }
            }
            return View(receivedItem);
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
