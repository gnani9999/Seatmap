// <copyright file="Cabin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.SeatMapModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Model Class for seat Cabin
    /// </summary>
    public class Cabin
    {
        public string CabinType { get; set; }

        public int RowCount { get; set; }

        public int AisleCount { get; set; }

        public int SeatCount { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}
