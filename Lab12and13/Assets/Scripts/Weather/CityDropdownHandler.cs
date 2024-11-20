using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityDropdownHandler : MonoBehaviour
{
    // List of cities
    private string[] cities = new string[]
    {
        "Seattle, US",        // Alaska, USA - Snow (morning/afternoon), UTC-9
        "Little Rock, AR",    // Canada - Rain (morning/afternoon), UTC-8
        "Reykjavik, Iceland", // Germany - Cloudy/Scattered clouds (morning/afternoon), UTC+1
        "Tokyo, JP",          // Japan - Clear Sky (morning/afternoon), UTC+9
        "Buenos Aires, Argentina" // Australia - Clear Sky or Cloudy (morning/afternoon), UTC+10
    };

    public TMP_Dropdown cityDropdown; // Reference to your Dropdown in the scene
    public TextMeshProUGUI weatherInfoText;
    public WeatherController weatherController;
    private WeatherManager weatherManager;

    void Start()
    {
        // Ensure the Dropdown is not null
        if (cityDropdown != null)
        {
            // Clear any existing options (in case the dropdown is reused)
            cityDropdown.options.Clear();

            // Add the cities to the dropdown
            foreach (string city in cities)
            {
                cityDropdown.options.Add(new TMP_Dropdown.OptionData(city));
            }

            // Optionally set the default selected value (optional)
            cityDropdown.value = 0; // Default to the first city, or use another value

            // Add a listener to detect when a new city is selected
            cityDropdown.onValueChanged.AddListener(OnCitySelected);
        }
        else
        {
            Debug.LogWarning("Dropdown is not assigned in the Inspector!");
        }
       weatherManager = new WeatherManager();
    }

    // This method will be called when a city is selected from the dropdown
    void OnCitySelected(int index)
    {
        string selectedCity = cities[index];
        Debug.Log("Selected City: " + selectedCity);
        weatherController.UpdateWeatherForCity(selectedCity);
        weatherInfoText.text = "Loading weather for: " + selectedCity;


    }
    public void OnWeatherDataLoaded(WeatherData weatherData)
    {
        DateTime currentTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(weatherData.Timezone));
        if (weatherData != null)
        {
            // Display weather data in the UI Text element
            weatherInfoText.text = $"Location: {weatherData.Location}\n" +
                                   $"Temperature: {weatherData.Temperature}°F\n" +
                                   $"Weather: {weatherData.WeatherDescription}\n" +
                                   $"Wind Speed: {weatherData.WindSpeed} m/s\n" +
                                   $"Sunrise: {weatherData.Sunrise.ToString("hh:mm tt")}\n" +
                                   $"Sunset: {weatherData.Sunset.ToString("hh:mm tt")}\n" +
                                   $"Current Time: {currentTime}";
        }
        else
        {
            weatherInfoText.text = "Failed to load weather data.";
        }
    }
}
