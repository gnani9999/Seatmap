// <copyright file="SeatMapItemType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.SeatMapModels
{
    using FileHelpers;

    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class SeatMapItemType
    {
        public string ItemType { get; set; }

        public string ItemTypeCode { get; set; }

        public string SeatCharacteristicCode { get; set; }

        public string SeatAttribute { get; set; }

        public string SeatType { get; set; }

        public string ItemDescription { get; set; }
    }
}
