using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioCalculator.Helpers;

public static class CSVHelper
{
    public static ReadOnlyCollection<T> GetListOfType<T>(string filePath)
    {
        ReadOnlyCollection<T> list;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            list = csv.GetRecords<T>().ToList().AsReadOnly();
        }        
        return list;
    }
}
