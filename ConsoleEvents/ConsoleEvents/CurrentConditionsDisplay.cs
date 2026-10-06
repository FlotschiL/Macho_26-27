namespace ObserverPattern;

internal sealed class CurrentConditionsDisplay
{
    public void OnConditionsChanged(object? sender, WeatherConditionsEventArgs e)
    {
        Console.WriteLine(
            $"[current] {e.Temperature:F1}°C, {e.Humidity}% humidity, {e.Pressure} hPa");
    }
}

