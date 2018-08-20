// <copyright file="SellableSeat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace SeatMapReference.SeatMapModels
{
    using FileHelpers;

    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class SellableSeat
    {
        public string SellableSeatCategory { get; set; }

        public string ProgrammingCode { get; set; }

        public string SeatType { get; set; }

        public string DisplaySeatType { get; set; }

        public string SharesSeatIndicator { get; set; }

        public string EDoc { get; set; }

        public string Description { get; set; }
    }
}
