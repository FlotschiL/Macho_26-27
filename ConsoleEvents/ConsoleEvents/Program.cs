namespace ObserverPattern;


internal static class Program
{
    private static void Main()
    {
        WeatherStation station = new();//the weather station is the publisher that raises the event
        CurrentConditionsDisplay current = new();//subscriber that only outputs the current conditions
        ForecastDisplay forecast = new();//a second subscriber that does calculations with the data (rain, no rain)
        //both need to be updated with the new sensor data each time it updates

        station.ConditionsChanged += current.OnConditionsChanged;
        station.ConditionsChanged += forecast.OnConditionsChanged;

        Console.WriteLine("1) Low pressure (990 hPa), both observers attached:");
        station.SetConditions(temperature: 20.0f, humidity: 80, pressure: 990);

        Console.WriteLine();
        Console.WriteLine("2) Pressure rises:");
        station.SetConditions(temperature: 22.5f, humidity: 65, pressure: 1015);

        Console.WriteLine();
        Console.WriteLine("3) Detach the forecast observer, then pressure drops (995):");
        station.ConditionsChanged -= forecast.OnConditionsChanged;
        station.SetConditions(temperature: 24.0f, humidity: 55, pressure: 995);

        Console.WriteLine();
        Console.WriteLine("Only [current] reacted in step 3 — the forecast observer was detached.");
    }
}







