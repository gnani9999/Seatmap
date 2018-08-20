// <copyright file="IFileHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.Helpers
{
    using System.Collections.Generic;

    public interface IFileHelper<T>
        where T : class
    {
        List<T> ReadFile(string filePath);

        string[] FileHeaders(int skip);
    }
}
