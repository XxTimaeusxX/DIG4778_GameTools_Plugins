using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager
{
    // Define the API endpoint URL, remember to replace "APIKEY" with your actual OpenWeatherMap API key
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?q=Orlando,us&mode=xml&appid=843876d8822b51aaad5173bf1e088795";

    // Private coroutine to handle the API request
    private IEnumerator CallAPI(string url, Action<WeatherData> callback)
    {
        // Start a Unity web request to get data from the API
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Wait until the web request is completed
            yield return request.SendWebRequest();

            // Handle connection errors
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Network problem: {request.error}");
            }
            // Handle response errors (e.g., 404, 500)
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Response error: {request.responseCode}");
            }
            // If everything is okay, pass the response to the callback function
            else
            {
                // Parse the raw XML and pass the WeatherData to the callback
                WeatherData weatherData = ParseWeatherXML(request.downloadHandler.text);
                callback(weatherData); // Send the parsed data to the callback
            }
        }
    }

    // Public method that you can call to start fetching weather data
    public IEnumerator GetWeatherXML(Action<WeatherData> callback)
    {
        // Simply call the internal CallAPI coroutine and return it
        return CallAPI(xmlApi, callback);
    }

    // Parse the weather data XML and return a WeatherData object
    private WeatherData ParseWeatherXML(string xmlData)
    {
        WeatherData weatherData = new WeatherData();
        XmlDocument xmlDoc = new XmlDocument();

        try
        {
            xmlDoc.LoadXml(xmlData);

            // Parse location
            XmlNode cityNode = xmlDoc.SelectSingleNode("current/city");
            weatherData.Location = cityNode.Attributes["name"]?.Value;

            // Parse temperature (in Kelvin, convert to Celsius or Fahrenheit)
            XmlNode tempNode = xmlDoc.SelectSingleNode("current/temperature");
            weatherData.Temperature = float.Parse(tempNode.Attributes["value"]?.Value ?? "0") - 273.15f; // Convert from Kelvin to Celsius

            // Parse weather description
            XmlNode weatherNode = xmlDoc.SelectSingleNode("current/weather");
            weatherData.WeatherDescription = weatherNode.Attributes["value"]?.Value;

            // Parse wind speed
            XmlNode windNode = xmlDoc.SelectSingleNode("current/wind");
            weatherData.WindSpeed = float.Parse(windNode.SelectSingleNode("speed")?.Attributes["value"]?.Value ?? "0");

            // Parse sunrise and sunset times
            XmlNode sunNode = xmlDoc.SelectSingleNode("current/city/sun");
            string sunriseStr = sunNode.Attributes["rise"]?.Value;
            string sunsetStr = sunNode.Attributes["set"]?.Value;
            weatherData.Sunrise = DateTime.Parse(sunriseStr);
            weatherData.Sunset = DateTime.Parse(sunsetStr);

            // Parse last update time
            XmlNode lastUpdateNode = xmlDoc.SelectSingleNode("current/lastupdate");
            weatherData.LastUpdate = DateTime.Parse(lastUpdateNode.Attributes["value"]?.Value);

            // Parse timezone offset (in seconds)
            XmlNode timezoneNode = xmlDoc.SelectSingleNode("current/city/timezone");
            weatherData.Timezone = timezoneNode?.InnerText;

            return weatherData;

        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing weather XML: {ex.Message}");
            return null;
        }
    }
}