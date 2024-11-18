using System;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Material[] skyboxes;  // Array to store your 15 skyboxes (assign these in the inspector)
    public Light directionalLight;
    private WeatherManager weatherManager;

    void Start()
    {
        
        weatherManager = new WeatherManager();  // Instantiate WeatherManager
        // Call the GetWeatherXML and pass the OnWeatherDataLoaded as a callback
        StartCoroutine(weatherManager.GetWeatherXML(OnWeatherDataLoaded));
    }

    // Callback method for when the weather data is loaded and parsed
    private void OnWeatherDataLoaded(WeatherData weatherData)
    {

        if (weatherData != null)
        {
            // Use the weather data, e.g., display it in the UI or adjust the scene
            Debug.Log($"Location: {weatherData.Location}");
            Debug.Log($"Temperature: {weatherData.Temperature}°f");
            Debug.Log($"Weather: {weatherData.WeatherDescription}");
            Debug.Log($"Wind Speed: {weatherData.WindSpeed} m/s");
            Debug.Log($"Last Update (UTC): {weatherData.LastUpdate}");
            // Log the Sunrise and Sunset times in local time
            DateTime currentTime = weatherData.LastUpdate.Add(TimeSpan.FromSeconds(int.Parse(weatherData.Timezone)));

            Debug.Log($"Current Time: {currentTime:hh:mm tt}");


            // Update the scene or UI based on the weather data
            UpdateScene(weatherData);
        }
    }

    // Example method to update the scene based on the weather data
    private void UpdateScene(WeatherData weatherData)
    {
        // Determine whether it's daytime or nighttime
        bool isDaytime = DateTime.Now >= weatherData.Sunrise && DateTime.Now <= weatherData.Sunset;
        SetSkyboxForWeather(weatherData, isDaytime);
        SetDirectionalLightForTimeOfDay(isDaytime);

    }

    // Set the skybox based on weather condition and time of day
    private void SetSkyboxForWeather(WeatherData weatherData, bool isDaytime)
    {
        // If it's day, set day-related skyboxes, otherwise, set night-related skyboxes
        if (isDaytime)
        {
            // Find the appropriate skybox based on weather conditions
            if (weatherData.WeatherDescription.Contains("clear"))
            {
                RenderSettings.skybox = skyboxes[0]; // Clear skybox (index can be adjusted based on your array)
            }
            else if (weatherData.WeatherDescription.Contains("cloud"))
            {
                RenderSettings.skybox = skyboxes[1]; // Cloudy skybox
            }
            else if (weatherData.WeatherDescription.Contains("rain"))
            {
                RenderSettings.skybox = skyboxes[2]; // Rainy skybox
            }
            // Add more conditions for other weather types...
        }
        else
        {
            // If it's night, set a nighttime skybox
            RenderSettings.skybox = skyboxes[3]; // Example of a night skybox (index can be adjusted)
        }
    }
    private void SetDirectionalLightForTimeOfDay(bool isDaytime)
    {
        if (directionalLight == null) return;

        if (isDaytime)
        {
            // Daytime lighting: bright and warm
            directionalLight.color = new Color(1f, 1f, 0.8f);  // Warm yellowish color
            directionalLight.intensity = 1f;  // Full intensity for daytime
            directionalLight.transform.rotation = Quaternion.Euler(50f, 0f, 0f);  // Sun's position
            Debug.LogWarning("its daytime");
        }
        else
        {
            // Nighttime lighting: dim and cool
            directionalLight.color = new Color(0f, 0f, 1f);  // Cool bluish color
            directionalLight.intensity = 1f;  // Dimmer light for nighttime
            directionalLight.transform.rotation = Quaternion.Euler(50f, 180f, 0f);
            Debug.LogWarning("its night time");
        }
    }

}