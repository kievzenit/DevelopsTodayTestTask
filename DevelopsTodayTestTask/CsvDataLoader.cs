namespace DevelopsTodayTestTask;

public sealed class CsvDataLoader(StreamReader streamReader)
{
    private readonly Dictionary<string, int> _columns = new();
    
    public IEnumerable<CabDataDto> Load()
    {
        var header = streamReader.ReadLine();
        if (header is null)
        {
            throw new MissingCsvHeaderException();
        }
        
        var columnNames = header.Split(',');
        for (var index = 0; index < columnNames.Length; index++)
        {
            var columnName = columnNames[index];
            _columns.Add(columnName, index);
        }

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line is null)
            {
                yield break;
            }

            if (line == string.Empty)
            {
                continue;
            }

            var values = line.Split(',');
            var tpepPickupDatetimeColumnValue = values[_columns["tpep_pickup_datetime"]];
            var tpepDropoffDatetimeColumnValue = values[_columns["tpep_dropoff_datetime"]];
            var passengerCountColumnValue = values[_columns["passenger_count"]];
            var tripDistanceColumnValue = values[_columns["trip_distance"]];
            var storeAndFwdFlagColumnValue = values[_columns["store_and_fwd_flag"]];
            var puLocationIdColumnValue = values[_columns["PULocationID"]];
            var doLocationIdColumnValue = values[_columns["DOLocationID"]];
            var fareAmountColumnValue = values[_columns["fare_amount"]];
            var tipAmountColumnValue = values[_columns["tip_amount"]];

            CabDataDto cabDto;
            try
            {
                cabDto = CabDataDtoFactory.Create(
                    tpepPickupDatetimeColumnValue,
                    tpepDropoffDatetimeColumnValue,
                    passengerCountColumnValue,
                    tripDistanceColumnValue,
                    storeAndFwdFlagColumnValue,
                    puLocationIdColumnValue,
                    doLocationIdColumnValue,
                    fareAmountColumnValue,
                    tipAmountColumnValue);
            }
            catch (ArgumentException)
            {
                continue;
            }

            yield return cabDto;
        }
    }
}
