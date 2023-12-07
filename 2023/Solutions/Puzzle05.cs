using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle05 : Puzzle2023<Puzzle05>
{
    private readonly List<Map> SeedToSoil = [];
    private readonly List<Map> SoilToFertiliser = [];
    private readonly List<Map> FertiliserToWater = [];
    private readonly List<Map> WaterToLight = [];
    private readonly List<Map> LightToTemperature = [];
    private readonly List<Map> TemperatureToHumidity = [];
    private readonly List<Map> HumidityToLocation = [];

    internal override void Part1()
    {
        var seeds = File[0].Split(' ')[1..].Select(long.Parse).ToArray();
        // seeds.Dump();

        MapType map = MapType.None;
        foreach (var line in File[2..])
        {
            var elements = line.Split(' ');
            if (elements.Length == 1)
            {
                // It's an empty line baby
                continue;
            }
            else if (elements.Length == 2)
            {
                map = GetMapType(elements[0]);
            }
            else
            {
                AddToMap(map, new Map()
                {
                    DestinationStart = long.Parse(elements[0]),
                    SourceStart = long.Parse(elements[1]),
                    Range = long.Parse(elements[2])
                });
            }
        }

        var minLocation = long.MaxValue;
        foreach (var seed in seeds)
        {
            var soilNumber = GetLocationFromMap(SeedToSoil, seed);
            var fertilserNumber = GetLocationFromMap(SoilToFertiliser, soilNumber);
            var waterNumber = GetLocationFromMap(FertiliserToWater, fertilserNumber);
            var lightNumber = GetLocationFromMap(WaterToLight, waterNumber);
            var temperatureNumber = GetLocationFromMap(LightToTemperature, lightNumber);
            var humidityNumber = GetLocationFromMap(TemperatureToHumidity, temperatureNumber);
            var locationNumber = GetLocationFromMap(HumidityToLocation, humidityNumber);

            minLocation = Math.Min(locationNumber, minLocation);

            // $"Seed {seed} -> Soil {soilNumber} -> Fertiliser {fertilserNumber} -> Water {waterNumber} -> Light {lightNumber} -> Temperature {temperatureNumber} -> Humidity {humidityNumber} -> Location {locationNumber}".Dump();
        }

        minLocation.Dump();
    }

    private static MapType GetMapType(string map) => map switch
    {
        "seed-to-soil" => MapType.SeedToSoil,
        "soil-to-fertilizer" => MapType.SoilToFertiliser,
        "fertilizer-to-water" => MapType.FertiliserToWater,
        "water-to-light" => MapType.WaterToLight,
        "light-to-temperature" => MapType.LightToTemperature,
        "temperature-to-humidity" => MapType.TemperatureToHumidity,
        "humidity-to-location" => MapType.HumidityToLocation,
        _ => throw new NotImplementedException("HUH HOW!"),
    };

    private void AddToMap(MapType mapType, Map map)
    {
        switch (mapType)
        {
            case MapType.SeedToSoil:
                SeedToSoil.Add(map);
                break;
            case MapType.SoilToFertiliser:
                SoilToFertiliser.Add(map);
                break;
            case MapType.FertiliserToWater:
                FertiliserToWater.Add(map);
                break;
            case MapType.WaterToLight:
                WaterToLight.Add(map);
                break;
            case MapType.LightToTemperature:
                LightToTemperature.Add(map);
                break;
            case MapType.TemperatureToHumidity:
                TemperatureToHumidity.Add(map);
                break;
            case MapType.HumidityToLocation:
                HumidityToLocation.Add(map);
                break;
        }
    }

    private long GetLocationFromMap(List<Map> map, long source)
    {
        var range = map.SingleOrDefault(r => source >= r.SourceStart && source < r.SourceStart + r.Range);
        return range is null
            ? source
            : range.DestinationStart + source - range.SourceStart;
    }

    internal override void Part2()
    {
        Part1();

        // Fuck...
    }

    public class Map
    {
        public long DestinationStart { get; set; }
        public long SourceStart { get; set; }
        public long Range { get; set; }
    }

    public enum MapType
    {
        None,
        SeedToSoil,
        SoilToFertiliser,
        FertiliserToWater,
        WaterToLight,
        LightToTemperature,
        TemperatureToHumidity,
        HumidityToLocation
    }
}
