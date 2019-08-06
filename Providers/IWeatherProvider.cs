using System.Collections.Generic;
using VuejsSignalR.Models;

namespace VuejsSignalR.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
