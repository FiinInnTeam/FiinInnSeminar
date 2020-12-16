using Microsoft.AspNetCore.Mvc;
using ProtocolBuf.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProtocolBuf.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly RedisCacheService _cacheService;
        private readonly WeatherForecastService _forecastService;

        public WeatherForecastController(RedisCacheService cacheService, WeatherForecastService forecastService)
        {
            _cacheService = cacheService;
            _forecastService = forecastService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var key = $"forecast-{DateTime.Today:dd/MM/yyyy}";

            var forcast = await _cacheService.GetAsync<IEnumerable<WeatherForecast>>(key);
            if (forcast == null)
            {
                forcast = _forecastService.GetWeatherForecasts();
                await _cacheService.SetAsync(key, forcast);
            }

            return forcast;
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetProtocolBuf()
        {
            var key = $"forecastprotbuf-{DateTime.Today:dd/MM/yyyy}";

            var forcast = await _cacheService.GetProtocolBufAsync<IEnumerable<WeatherForecast>>(key);
            if (forcast == null)
            {
                forcast = _forecastService.GetWeatherForecasts();

                await _cacheService.SetProtocolBufAsync(key, forcast);
            }

            return forcast;
        }
    }
}
