namespace ObserverPattern;

/// <summary>
/// Demo of the observer pattern — implemented without C# events.
///
/// The station (subject) knows nothing about the displays (observers);
/// it only keeps a list of IObserver and calls Notify() on state changes.
/// </summary>
internal static class Program
{
    private static void Main()
    {
        WeatherStation station = new();//the weather station is the concrete subject that is observed by the 2 Observers
        CurrentConditionsDisplay current = new();//is an observer that only outputs the current conditions
        ForecastDisplay forecast = new();//a second observer that does calculations with the data (rain, no rain)
        //both need to be updated with the new sensor data each time it updates

        station.Attach(current);
        station.Attach(forecast);

        Console.WriteLine("1) Low pressure (990 hPa), both observers attached:");
        station.SetConditions(temperature: 20.0f, humidity: 80, pressure: 990);

        Console.WriteLine();
        Console.WriteLine("2) Pressure rises:");
        station.SetConditions(temperature: 22.5f, humidity: 65, pressure: 1015);

        Console.WriteLine();
        Console.WriteLine("3) Detach the forecast observer, then pressure drops (995):");
        station.Detach(forecast);
        station.SetConditions(temperature: 24.0f, humidity: 55, pressure: 995);

        Console.WriteLine();
        Console.WriteLine("Only [current] reacted in step 3 — the forecast observer was detached.");
    }
}
