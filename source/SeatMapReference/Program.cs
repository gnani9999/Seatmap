
// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference
{
    using System.IO;
    using Helpers;
    using Microsoft.Extensions.DependencyInjection;
    using SeatMapModels;

    internal class Program
    {
        private static void Main()
        {
            var serviceProvider = new ServiceCollection()
             .AddSingleton(typeof(IFileHelper<>), typeof(FileHelper<>))
             .AddSingleton(typeof(ISeatMapValuesMap), typeof(SeatMapValuesMap))
             .BuildServiceProvider();

            const string csvFilePath = "../../../../../doc//CsvFile";

            const string jsonFilePath = "../../../../../doc//JsonObject//";

            const string sellableSeatsFilePath = "../../../../../doc/SellableSeats/SellableSeats.csv";

            const string seatMapItemTypesFilePath = "../../../../../doc/SellableSeats/SeatMapItemTypes.csv";

            const string csvExtn = "*.csv";

            const string jsonExtn = ".json";

            var sellableSeatService = serviceProvider.GetService<IFileHelper<SellableSeat>>();
            var flightSeatMapService = serviceProvider.GetService<IFileHelper<FlightSeatMap>>();
            var seatMapItemTypeService = serviceProvider.GetService<IFileHelper<SeatMapItemType>>();
            var seatMapValueService = serviceProvider.GetService<ISeatMapValuesMap>();

            var filePaths = Directory.GetFiles(csvFilePath, csvExtn, SearchOption.AllDirectories);
            var sellableSeats = sellableSeatService.ReadFile(sellableSeatsFilePath);
            var seatMapItemTypes = seatMapItemTypeService.ReadFile(seatMapItemTypesFilePath);
            foreach (var file in filePaths)
            {
                var flightSeatMaps = flightSeatMapService.ReadFile(file);
                var seatMaps = seatMapValueService.GetSeatMap(flightSeatMaps, sellableSeats, seatMapItemTypes);
                var jsonString = seatMapValueService.ObjectToJson(seatMaps);
                seatMapValueService.WriteToFile(jsonString, jsonFilePath + Path.GetFileNameWithoutExtension(file) + jsonExtn);
            }
        }
    }
}
