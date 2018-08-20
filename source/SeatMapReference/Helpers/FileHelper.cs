// <copyright file="FileHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SeatMapReference.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FileHelpers;

    public class FileHelper<T> : IFileHelper<T>
        where T : class
    {
        public List<T> ReadFile(string filePath)
        {
            try
            {
                var fileEngine = new FileHelperEngine<T>();
                return fileEngine.ReadFile(filePath).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Reading File :: " + ex);
                throw;
            }
        }

        public string[] FileHeaders(int skip)
        {
            var fileEngine = new FileHelperEngine<T>();
            return fileEngine.Options.FieldsNames.Skip(skip).ToArray();
        }
    }
}
