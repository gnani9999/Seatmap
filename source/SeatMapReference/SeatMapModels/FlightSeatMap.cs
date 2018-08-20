// <copyright file="FlightSeatMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace SeatMapReference.SeatMapModels
{
    using FileHelpers;

    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class FlightSeatMap
    {
        public string SeatMap { get; set; }

        public string VerticalGridNumber { get; set; }

        public string HorizontalGridNumber { get; set; }

        public string ItemType { get; set; }

        public string SeatNumber { get; set; }

        public string SeatLetter { get; set; }

        public string CabinType { get; set; }

        public string SellableSeatCategory { get; set; }

        public string PermanentlyBlocked { get; set; }

        public string Sellable { get; set; }

        public string EliteZone { get; set; }

        public string Bulkhead { get; set; }

        public string Exit { get; set; }

        public string LimitedRecline { get; set; }

        public string MoveableArmrest { get; set; }

        public string Aisle { get; set; }

        public string Middle { get; set; }

        public string Window { get; set; }

        public string LeftAisle { get; set; }

        public string LeftMiddle { get; set; }

        public string LeftWindow { get; set; }

        public string RightAisle { get; set; }

        public string RightMiddle { get; set; }

        public string RightWindow { get; set; }

        public string CenterAisle { get; set; }

        public string CenterMiddle { get; set; }

        public string Under15YrsOld { get; set; }

        public string PetInCabin { get; set; }

        public string PrisonerGuard { get; set; }

        public string DisabledPassenger { get; set; }

        public string LapInfant { get; set; }

        [FieldOptional]
        public string Blocked { get; set; }
    }
}
