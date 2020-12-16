using BenchmarkDotNet.Attributes;
using ProtocolBuf.Services;
using System.Collections.Generic;
using System.IO;

namespace ProtocolBuf.Benchmark
{
    public class SerializationBenchmark
    {
        private static readonly WeatherForecastService _weatherForecastService = new WeatherForecastService();

        [Benchmark]
        public List<WeatherForecast> NewtonSoftJsonSerialize()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeatherForecast>>(Newtonsoft.Json.JsonConvert.SerializeObject(_weatherForecastService.GetWeatherForecasts()));
        }

        [Benchmark]
        public List<WeatherForecast> JsonSerialize()
        {
            return System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(System.Text.Json.JsonSerializer.Serialize(_weatherForecastService.GetWeatherForecasts()));
        }

        [Benchmark]
        public List<WeatherForecast> ProtobufSerialize()
        {
            using var stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(stream, _weatherForecastService.GetWeatherForecasts());
            return ProtoBuf.Serializer.Deserialize<List<WeatherForecast>>(stream);
        }
    }
}
