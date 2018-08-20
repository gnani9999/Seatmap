// <copyright file="SeatMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.SeatMapModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Model class for SeatMap
    /// </summary>
    public class SeatMap
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public int NumberOfCabins { get; set; }

        public List<Cabin> Cabins { get; set; } = new List<Cabin>();
    }
}
