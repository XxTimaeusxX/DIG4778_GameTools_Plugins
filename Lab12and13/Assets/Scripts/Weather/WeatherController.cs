using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherController;

public class WeatherController : MonoBehaviour
{
    public Material[] skyboxes;  // Array to store your 15 skyboxes (assign these in the inspector)
    public Light directionalLight;
    private WeatherManager weatherManager;
    public enum TimeOfDay
    {
        Morning,   // Sunrise to Noon
        Afternoon, // Noon to Sunset
        Evening,   // Sunset to Night
        Night      // Night time (After sunset until next sunrise)
    }

    void Start()
    {
        weatherManager = new WeatherManager();  // Instantiate WeatherManager       
    }
    public void UpdateWeatherForCity(string city)
    {
        // Log the selected city
        Debug.Log("Updating weather for city: " + city);

        // Call the GetWeatherXML method with the selected city and a callback method
        StartCoroutine(weatherManager.GetWeatherXML(city, OnWeatherDataLoaded));
    }

    // Callback method for when the weather data is loaded and parsed
    private void OnWeatherDataLoaded(WeatherData weatherData)
    {

        if (weatherData != null)
        {
           

            // Get the time of day (Morning, Afternoon, Evening, Night)
            TimeOfDay timeOfDay = GetTimeOfDay(weatherData);
            SetSkyboxForWeather(weatherData, timeOfDay);
            SetDirectionalLightForTimeOfDay(timeOfDay, weatherData);

            FindObjectOfType<CityDropdownHandler>().OnWeatherDataLoaded(weatherData);
        }
    }

    private TimeOfDay GetTimeOfDay(WeatherData weatherData)
    {
        DateTime localTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(weatherData.Timezone));
        DateTime morning = weatherData.Sunrise.AddMinutes(-30); // 30 minutes before sunrise
        DateTime afternoon = weatherData.Sunrise.AddMinutes(5 * 60); // 6 hours after sunrise
        DateTime evening = weatherData.Sunset.AddMinutes(-40); // 1 hour before sunset
        DateTime night = weatherData.Sunset.AddMinutes(60); // 1 hour after sunset

        // Check the time of day
        if (localTime >= morning && localTime < afternoon)
        {
            Debug.Log("morning");
            return TimeOfDay.Morning;
        }
        else if (localTime >= afternoon && localTime < evening)
        {
            Debug.Log("afternoon");
            return TimeOfDay.Afternoon;
        }
        else if (localTime >= evening && localTime < night)
        {
            Debug.Log("Evening");
            return TimeOfDay.Evening;
        }
        else
        {
            Debug.Log("night");
            return TimeOfDay.Night;
        }
        
    }

    // Set the skybox based on weather condition and time of day
    private void SetSkyboxForWeather(WeatherData weatherData, TimeOfDay timeOfDay)
    {
           // Weather conditions dictionary mapping weather description to skybox index
        Dictionary<string, int> weatherConditions = new Dictionary<string, int>
        {
            { "clear", 1 },              // Clear Sky -> Day Sky
                           
            { "rain", 4 },                   // Rain -> Rainy
            { "snow", 5 },                   // Snow -> Snowy
            { "thunderstorm", 6 },           // Thunderstorm -> Thunderstorm
            { "fog", 8 },
            {"mist", 8 },  
            { "cloud", 7 },             // Few Clouds -> Cloudy
                
        };
            bool weatherFound = false;
            foreach (var condition in weatherConditions)
            {
                if (weatherData.WeatherDescription.Contains(condition.Key))
                {
                    Debug.Log("weather Found");
                    RenderSettings.skybox = skyboxes[condition.Value]; // Assign corresponding skybox
                    weatherFound = true;
                    break;
                }
            }

            // Default to clear sky if no specific weather found
            if (!weatherFound)
            {
                Debug.Log("no weather found");
                RenderSettings.skybox = skyboxes[0]; // Default to clear sky (index 0)
            }
     //   SetDirectionalLightForTimeOfDay(timeOfDay, weatherData);
    }
  
    private void SetDirectionalLightForTimeOfDay(TimeOfDay timeOfDay, WeatherData weatherData)
    {
        Material skyboxMaterial = RenderSettings.skybox;
        float exposure = 1f;
        Color tintColor = new Color(0.5f,0.5f,0.5f);

        if (directionalLight == null) return;
        float intensityMultiplier = 1f;
        switch (timeOfDay)
        {
            case TimeOfDay.Morning:
                directionalLight.color = new Color(1f, 0.4f, 0f);  // Warm light, like early morning
                directionalLight.intensity = 1.55f;  
                directionalLight.transform.rotation = Quaternion.Euler(9f, 75f, 0f);  // Low sun angle
                tintColor = new Color(.6f, 0.27f, 0.22f);
                intensityMultiplier = .5f;
                break;
            case TimeOfDay.Afternoon:
                directionalLight.color = new Color(1f, 1f, 1f);  // Bright, midday sun
                directionalLight.intensity = 1f;  
                directionalLight.transform.rotation = Quaternion.Euler(90f, 90f, 0f);  // Direct overhead sun
                tintColor = new Color(0.5f, 0.5f, 0.5f);
                break;
            case TimeOfDay.Evening:
                directionalLight.color = new Color(1f, 0.6f, 0.3f);  // Warm golden-hour light
                directionalLight.intensity = 2f;  
                directionalLight.transform.rotation = Quaternion.Euler(6f, 72f, 0f);  // Low angle for sunset
                tintColor = new Color(.36f, 0.21f, 0.12f);
                intensityMultiplier = 0.22f;
                break;
            case TimeOfDay.Night:
                directionalLight.color = new Color(0f, 0f, 0.7f);  // Dark, nighttime color
                directionalLight.intensity = 1f;  
                directionalLight.transform.rotation = Quaternion.Euler(17f, 75f, 0f);  // Nighttime (direction of light doesn't matter)
                tintColor = new Color(0.05f, 0.05f, 0.22f);
                exposure = 0.2f;
                intensityMultiplier = 0.7f;
                break;
        }
        if (weatherData.WeatherDescription.Contains("rain") || weatherData.WeatherDescription.Contains("snow") ||
            weatherData.WeatherDescription.Contains("fog") || weatherData.WeatherDescription.Contains("mist"))
        {
            directionalLight.intensity *= 0.2f;  // Lower intensity for rainy or snowy weather
            skyboxMaterial.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
            exposure = 0.8f;
        }
        // Apply the exposure, saturation, and tint adjustments to the skybox material
        skyboxMaterial.SetFloat("_Exposure", exposure);
        skyboxMaterial.SetColor("_Tint", tintColor);
        RenderSettings.ambientIntensity = directionalLight.intensity * intensityMultiplier;

    }

}