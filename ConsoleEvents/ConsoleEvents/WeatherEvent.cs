internal sealed class WeatherConditionsEventArgs : EventArgs
{
    public required float Temperature { get; init; }
    public required int Humidity { get; init; }
    public required int Pressure { get; init; }
}