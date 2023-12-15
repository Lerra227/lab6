using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;


List<Weather> weathers = new List<Weather>(60);

for(int i = 0; i < 60; i++)
{
    Weather weather = new Weather();
    weather.init();
    weathers.Add(weather);
}


var min = weathers.OrderBy(w => w.Temp).First();
Console.WriteLine($"Min temp {min.Country} {min.Name}");
var max = weathers.OrderBy(w => w.Temp).Last();
Console.WriteLine($"Max temp {max.Country} {max.Name}");
var avr = weathers.Average(w => w.Temp);
Console.WriteLine($"Average temp {avr}");
var count = weathers.Select(w => w.Country).Distinct().Count();
Console.WriteLine($"Count {count}");
var result = weathers.FirstOrDefault(w => w.Description == "clear sky" ||  w.Description == "rain" || w.Description == "few clouds");
Console.WriteLine($"Result {result.Country}");
Console.ReadLine();





class CountryComparer : IEqualityComparer<Weather> 
{
    public bool Equals(Weather x, Weather y)
    {
        if (Object.ReferenceEquals(x, y)) return true;
        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;
        return x.Country == y.Country;
    }
    public int GetHashCode(Weather x)
    {
        //Check whether the object is null
        if (Object.ReferenceEquals(x, null)) return 0;

        //Get hash code for the Name field if it is not null.
        int hashWeatherCouhtry = x.Country == null ? 0 : x.Country.GetHashCode();
        //Calculate the hash code for the product.
        return hashWeatherCouhtry;
    }
}







public struct Weather
{
    public string Country { get; set; }
    public string Name { get; set; }
    public double Temp { get; set; }
    public string Description { get; set; }

    public void init()
    {
        bool HasNameOrCountry = false;

        while(!HasNameOrCountry)
        {
            try
            {

                HttpClient client = new HttpClient();
                var rand = new Random();
                double lat = rand.NextDouble() * (90 + 90) - 90;
                double lon = rand.NextDouble() * (180 + 180) - 180;
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=bf6581304f1ef89d28112c79ebf17265";
                // string url = "https://api.openweathermap.org/data/2.5/weather?lat=lat&lon=lon&units=metric&appid=bf6581304f1ef89d28112c79ebf17265";

                string response = client.GetStringAsync(url).Result;
                JObject JObject = JObject.Parse(response);
                if ((string)JObject["name"] != "" || (string)JObject["sys"]["country"] != null )
                {
                    Country = (string)JObject["sys"]["country"];
                    Name = (string)JObject["name"];
                    Temp = (double)JObject["main"]["temp"];
                    Description = (string)JObject["weather"][0]["description"];
                    HasNameOrCountry = true;
                }

            }
            catch
            { }
          
        }
       
        
    }

}