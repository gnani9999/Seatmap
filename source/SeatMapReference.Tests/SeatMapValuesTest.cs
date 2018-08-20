// <copyright file="SeatMapValuesTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helpers;
    using SeatMapModels;
    using Xunit;

    public class SeatMapValuesTest
    {
        private const string SellableSeatFilePath = "../../../../../doc/SellableSeats/SellableSeats.csv";

        private const string SeatMapItemTypesFilePath = "../../../../../doc/SellableSeats/SeatMapItemTypes.csv";

        private readonly IFileHelper<SellableSeat> sellableSeatHelper;
        private readonly IFileHelper<FlightSeatMap> flightSeatHelper;
        private readonly IFileHelper<SeatMap> seatMapHelper;
        private readonly IFileHelper<SeatMapItemType> seatMapItemTypeHelper;
        private readonly ISeatMapValuesMap seatMapValuesService;

        public SeatMapValuesTest()
        {
            this.sellableSeatHelper = new FileHelper<SellableSeat>();
            this.seatMapItemTypeHelper = new FileHelper<SeatMapItemType>();
            this.flightSeatHelper = new FileHelper<FlightSeatMap>();
            this.SellableSeats = new List<SellableSeat>();
            this.FlightSellableSeatMaps = new List<FlightSeatMap>();
            this.seatMapHelper = new FileHelper<SeatMap>();
            this.SellableSeats = this.sellableSeatHelper.ReadFile(SellableSeatFilePath);
            this.SeatMapItemTypes = this.seatMapItemTypeHelper.ReadFile(SeatMapItemTypesFilePath);
            this.seatMapValuesService = new SeatMapValuesMap(this.flightSeatHelper, this.seatMapHelper);
        }

        private List<SellableSeat> SellableSeats { get; set; }

        private List<SeatMapItemType> SeatMapItemTypes { get; set; }

        private List<FlightSeatMap> FlightSellableSeatMaps { get; set; }

        [Fact]
        public void ReadFileInValidPathFailTest()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//CsvFile//FileDontExist.csv";

            // Then
            Assert.Throws<FileNotFoundException>(() => this.flightSeatHelper.ReadFile(filePath));
        }

        [Fact]
        public void ReadFileValidPathTest()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//CsvFile//U7R.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains("Id", result);
            Assert.Contains("Cabins", result);
            Assert.Contains("CabinType", result);
            Assert.Contains("Seats", result);
            Assert.Contains("Characteristics", result);
        }

        [Fact]
        public void WriteFileValidTest()
        {
            // Given
            string jsonString = "{\r\n  \"Description\": \"U7R\",\r\n  \"NumberOfCabins\": 3,\r\n  \"Cabins\": [\r\n    {\r\n      \"CabinType\": \"F\",\r\n      \"Items\": [\r\n        {\r\n          \"VerticleGridNumber\": 1000\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            string jsonFilePath = "../../../../../doc//TestDocuments//JsonObject//SeatMap.json";

            // Then
            var ex = Record.Exception(() => this.seatMapValuesService.WriteToFile(jsonString, jsonFilePath));
            Assert.Null(ex);
        }

        [Fact]
        public void WriteFileInValidTest()
        {
            // Given
            string jsonString = "{\r\n  \"Description\": \"U7R\",\r\n  \"NumberOfCabins\": 3,\r\n  \"Cabins\": [\r\n    {\r\n      \"CabinType\": \"F\",\r\n      \"Items\": [\r\n        {\r\n          \"VerticleGridNumber\": 1000\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            string jsonFilePath = "../../../../../doc//TestDocuments//JsonObjectTest//SeatMap.json";

            // Then
            Assert.Throws<DirectoryNotFoundException>(() => this.seatMapValuesService.WriteToFile(jsonString, jsonFilePath));
        }

        [Fact]
        public void SeatTypeforSellableSeatTest()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellablePropertySheet.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SeatType\": \"EPlusPrimePlus\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SeatTypeforExitRowSeatTest()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellableSeats.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SeatType\": \"Exit\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SeatTypeforBulkheadSeat()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellableSeats.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SeatType\": \"Bulkhead\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void NonSeatItemTypeforBulkhead()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//NonSeatItem.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.DoesNotContain(" \"SeatType\": \"Seat\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SeatTypeforBlockedSeat()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellableSeats.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SeatType\": \"Blocked\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SeatTypeforStandardSeat()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellableSeats.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SeatType\": \"Seat\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SharesSeatIndicatorfromSeatType()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellablePropertySheet.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SharesSeatIndicator\": \"V\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void SharesSeatIndicatorforExitRowSeat()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellableSeats.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"SharesSeatIndicator\": \"E\"", result, StringComparison.CurrentCulture);
        }

        [Fact]
        public void DisplaySeatTypeForBlockedSeatTest()
        {
            // Given
            string filePath = "../../../../../doc//TestDocuments//SellableSeats//SellablePropertySheet.csv";

            // When
            var flightSeats = this.flightSeatHelper.ReadFile(filePath);
            var seats = this.seatMapValuesService.GetSeatMap(flightSeats, this.SellableSeats, this.SeatMapItemTypes);
            string result = this.seatMapValuesService.ObjectToJson(seats);

            // Then
            Assert.Contains(" \"DisplaySeatType\": \"\"", result, StringComparison.CurrentCulture);
        }

    }
}
