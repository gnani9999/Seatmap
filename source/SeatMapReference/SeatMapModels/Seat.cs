// <copyright file="Seat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.SeatMapModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Model Class for Seats
    /// </summary>
    public class Seat
    {
        public int VerticalGridNumber { get; set; }

        public int HorizontalGridNumber { get; set; }

        public string ItemType { get; set; }

        public string SeatCharacteristicCode { get; set; }

        public string SeatType { get; set; }

        public int SeatNumber { get; set; }

        public string SeatLetter { get; set; }

        public string SharesSeatIndicator { get; set; }

        public string CabinType { get; set; }

        public string ProgramPricingCode { get; set; }

        public string Description { get; set; }

        public string DisplaySeatType { get; set; }

        public string EDoc { get; set; }

        public string ItemTypeCode { get; set; }

        public string SeatAttribute { get; set; }

        public string ItemDescription { get; set; }

        public List<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
    }
}
