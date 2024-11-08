using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WeatherAPI.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetWeather(string city)
        {
            if(string.IsNullOrEmpty(city))
            {
                ViewBag.Error = "Please enter the city name";
                return View("Index");
            }
            string apiKey = "d3d49f8ca05b6c72690ca8000d5a055e"; //ключ
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ru";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject weatherData = JObject.Parse(responseBody);

                    ViewBag.City = weatherData["name"].ToString();
                    ViewBag.Description = weatherData["weather"][0]["description"].ToString();
                    ViewBag.Temperature = weatherData["main"]["temp"].ToString();
                    ViewBag.Humidity = weatherData["main"]["humidity"].ToString();
                }
                catch
                {
                    ViewBag.Error = "Произошла ошибка при получении данных о погоде.";
                }
            }
            return View("Index");
        }
    }
}
