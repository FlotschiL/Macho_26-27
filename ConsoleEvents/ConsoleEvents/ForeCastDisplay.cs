namespace ObserverPattern;

internal sealed class ForecastDisplay
{
    public void OnConditionsChanged(object? sender, WeatherConditionsEventArgs e)
    {
        string outlook = e.Pressure < 1000 ? "Rain expected" : "No rain expected";
        Console.WriteLine($"[forecast] {outlook} (pressure {e.Pressure} hPa)");
    }
}