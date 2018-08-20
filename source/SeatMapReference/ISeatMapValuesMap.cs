// <copyright file="ISeatMapValuesMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference
{
    using System.Collections.Generic;
    using SeatMapModels;

    public interface ISeatMapValuesMap
    {
        SeatMap GetSeatMap(List<FlightSeatMap> flightSeatMapValues, List<SellableSeat> sellableSeatValues, List<SeatMapItemType> seatMapItemTypes);

        string ObjectToJson(object list);

        void WriteToFile(string inputString, string location);
    }
}
