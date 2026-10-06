namespace ObserverPattern;

internal sealed class WeatherStation
{
    public event EventHandler<WeatherConditionsEventArgs>? ConditionsChanged;

    public void SetConditions(float temperature, int humidity, int pressure)
    {
        ConditionsChanged?.Invoke(this, new WeatherConditionsEventArgs
        {
            Temperature = temperature,
            Humidity = humidity,
            Pressure = pressure
        });
    }
}