// <copyright file="Characteristic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.SeatMapModels
{
    /// <summary>
    /// Model class for seat characteristics
    /// </summary>
    public class Characteristic
    {
        private const string IsCharacteristicTrue = "X";

        public Characteristic(string value, string header)
        {
            this.Key = header;

            this.Value = (value != IsCharacteristicTrue) ? value : "true";
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
