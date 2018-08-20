// <copyright file="SeatMapValuesMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Helpers;
    using Newtonsoft.Json;
    using SeatMapModels;

    public class SeatMapValuesMap : ISeatMapValuesMap
    {
        private const string ItemTypeAisle = "AISLE";

        private const string BulkheadSeatType = "Bulkhead";

        private const string ExitSeatType = "Exit";

        private const string BlockedSeatType = "Blocked";

        private const string SeatSeatType = "Seat";

        private const string ItemTypeDiscarded = "DISCARDED";

        private const int CharactersticStartIndex = 7;

        private readonly IFileHelper<SeatMap> seatMapHelper;

        private readonly IFileHelper<FlightSeatMap> flightSeatMapHelper;

        public SeatMapValuesMap(IFileHelper<FlightSeatMap> flightSeatMapHelper, IFileHelper<SeatMap> seatMapHelper)
        {
            this.seatMapHelper = seatMapHelper;
            this.flightSeatMapHelper = flightSeatMapHelper;
        }

        private List<SellableSeat> SellableSeats { get; set; }

        private List<SeatMapItemType> SeatMapItemTypes { get; set; }

        public SeatMap GetSeatMap(List<FlightSeatMap> flightSeatMapValues, List<SellableSeat> sellableSeatValues, List<SeatMapItemType> seatMapItemTypes)
        {
            this.SellableSeats = sellableSeatValues;
            this.SeatMapItemTypes = seatMapItemTypes;
            var seats = this.MapSeats(flightSeatMapValues);
            var cabinTypes = seats.Select(x => x.CabinType).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            var cabinList = cabinTypes.Select(cabinType => this.MapCabin(cabinType, seats)).ToList();
            var seatMap = new SeatMap
            {
                Description = flightSeatMapValues.FirstOrDefault()?.SeatMap
            };
            seatMap.Cabins.AddRange(cabinList);
            seatMap.NumberOfCabins = cabinList.Count;
            return seatMap;
        }

        public List<Seat> MapSeats(List<FlightSeatMap> flightSeatMapValues)
        {
            return flightSeatMapValues.Select(this.MapSeat).ToList();
        }

        public Seat MapSeat(FlightSeatMap seat)
        {
            var result = new Seat()
            {
                CabinType = seat.CabinType,
                Characteristics = this.MapCharacterstics(seat),
                HorizontalGridNumber = Convert.ToInt32(seat.HorizontalGridNumber, CultureInfo.CurrentCulture),
                ItemType = seat.ItemType,
                SeatLetter = seat.SeatLetter,
                SeatNumber = !string.IsNullOrWhiteSpace(seat.SeatNumber) ? Convert.ToInt32(seat.SeatNumber, CultureInfo.CurrentCulture) : 0,
                VerticalGridNumber = Convert.ToInt32(seat.VerticalGridNumber, CultureInfo.CurrentCulture),
            };
            result = this.MapSeatType(result);
            return result;
        }

        public Seat MapSeatItemTypeProperties(Seat result)
        {
            var seatMapItemType = this.SeatMapItemTypes
                .FirstOrDefault(i => i.ItemType.ToLower(CultureInfo.CurrentCulture) == result.ItemType.ToLower(CultureInfo.CurrentCulture));
            result.SeatCharacteristicCode = seatMapItemType?.SeatCharacteristicCode;
            return result;
        }

        public Seat MapSeatItemProperties(Seat result)
        {
            var seatItemType = this.SeatMapItemTypes
                .FirstOrDefault(i => i.ItemType.ToLower(CultureInfo.CurrentCulture) == result.ItemType.ToLower(CultureInfo.CurrentCulture));
            result.ItemTypeCode = seatItemType?.ItemTypeCode;
            result.SeatAttribute = seatItemType?.SeatAttribute;
            result.ItemDescription = seatItemType?.ItemDescription;
            return result;
        }

        public Seat MapSeatType(Seat seat)
        {
            var sellableCharacteristic = seat.Characteristics.FirstOrDefault(characteristic => characteristic.Key == "SellableSeatCategory");

            var seatTypeMap = new[]
            {
                new { SeatType = ExitSeatType }, new { SeatType = BulkheadSeatType }, new { SeatType = BlockedSeatType }
            };

            var sellableCategory = new List<string>();
            foreach (SellableSeat sellable in this.SellableSeats)
            {
                sellableCategory.Add(sellable.SellableSeatCategory);
                sellableCategory = sellableCategory.Where(x => !string.IsNullOrEmpty(x)).ToList();
            }

            if (sellableCharacteristic != null && sellableCategory.Contains(sellableCharacteristic.Value, StringComparer.OrdinalIgnoreCase))
            {
                return this.SetSellableSeatCharacteristic(sellableCharacteristic, seat);
            }

            if (sellableCharacteristic != null && !sellableCategory.Contains(sellableCharacteristic.Value, StringComparer.OrdinalIgnoreCase))
            {
                var badSellableSeat = sellableCharacteristic.Value;
                throw new ArgumentException("{0} value is BADSellableSeatCategory", badSellableSeat);
            }

            if (seatTypeMap.FirstOrDefault(type => seat.Characteristics.Any(x => x.Key == type.SeatType)) != null)
            {
                seat.SeatType = seat.SeatNumber != 0 ? seatTypeMap
                    .FirstOrDefault(type => seat.Characteristics.Any(x => x.Key == type.SeatType))?.SeatType : seat.SeatType;
                return this.SetSeatProperties(seat);
            }

            if (seat.SeatNumber != 0)
            {
                seat.SeatType = SeatSeatType;
                seat = this.SetSeatProperties(seat);
                seat = this.MapSeatItemTypeProperties(seat);
            }

            if (!string.IsNullOrEmpty(seat.ItemType))
            {
                seat = this.MapSeatItemProperties(seat);
                if (seat.SeatNumber == 0)
                {
                    seat.SeatType = seat.ItemType;
                }
            }

            return seat;
        }

        public Seat SetSeatProperties(Seat seat)
        {
            SellableSeat sellableSeat = this.SellableSeats.FirstOrDefault(x => x.SeatType == seat.SeatType);
            if (sellableSeat != null)
            {
                seat.SharesSeatIndicator = sellableSeat.SharesSeatIndicator;
                seat.ProgramPricingCode = sellableSeat.ProgrammingCode;
                seat.Description = sellableSeat.Description;
                seat.DisplaySeatType = sellableSeat.DisplaySeatType;
                seat.EDoc = !string.IsNullOrEmpty(sellableSeat.EDoc) ? sellableSeat.EDoc.Split("/")[0] : sellableSeat.EDoc;
            }

            return seat;
        }

        public Seat SetSellableSeatCharacteristic(Characteristic sellableCharacteristic, Seat seat)
        {
            var sellablePropertyName = sellableCharacteristic.Value.ToLower(CultureInfo.CurrentCulture);
            var sellableSeatProperties = this.SellableSeats
                .FirstOrDefault(sellableSeat => sellableSeat.SellableSeatCategory.ToLower(CultureInfo.CurrentCulture) == sellablePropertyName);
            if (sellableSeatProperties != null)
            {
                seat.SeatType = sellableSeatProperties.SeatType;
                seat = this.SetSeatProperties(seat);
            }

            return seat;
        }

        public string ObjectToJson(object list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public void WriteToFile(string inputString, string location)
        {
            try
            {
                using (var sw = new StreamWriter(location))
                {
                    sw.Write(inputString);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception in Writing file" + ex);
                throw;
            }
        }

        public List<Characteristic> MapCharacterstics(FlightSeatMap flightSeatMap)
        {
            var chararcteristicProps = this.flightSeatMapHelper.FileHeaders(CharactersticStartIndex);
            return flightSeatMap.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => chararcteristicProps.Contains(i.Name) && !string.IsNullOrWhiteSpace(i.GetValue(flightSeatMap, null).ToString()))
            .Select(prop => new Characteristic(prop.GetValue(flightSeatMap, null).ToString(), prop.Name)).ToList();
        }

        public Cabin MapCabin(string cabinType, List<Seat> seatList)
        {
            var cabin = new Cabin { CabinType = cabinType };
            cabin.Seats.AddRange(seatList.Where(seat => seat.CabinType == cabinType && seat.ItemType != ItemTypeDiscarded));
            cabin.AisleCount =
                seatList.Count(seat => seat.CabinType == cabinType && seat.ItemType == ItemTypeAisle);
            cabin.SeatCount = cabin.Seats.Count();
            return cabin;
        }
    }
}
