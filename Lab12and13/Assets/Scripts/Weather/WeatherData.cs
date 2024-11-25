using System;

public class WeatherData
{
    public string Location { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public float Temperature { get; set; } 
    public float Humidity { get; set; }
    public string WeatherDescription { get; set; }
    public float WindSpeed { get; set; }
    public string WindDirection { get; set; }
    public DateTime Sunrise { get; set; }
    public DateTime Sunset { get; set; }
    public DateTime LastUpdate { get; set; }
    public int Timezone { get; set; }

    // Optionally, you can add methods to perform actions based on the data (e.g., updating lighting)
}