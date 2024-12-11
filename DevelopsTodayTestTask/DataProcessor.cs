using System.Collections.Concurrent;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DevelopsTodayTestTask;

public sealed class DataProcessor(IEnumerable<CabDataDto> data, string connectionString)
{
    private readonly ConcurrentDictionary<(DateTime, DateTime, int), bool> _duplicates = new();
    private readonly ConcurrentBag<CabDataDto> _duplicateData = new();

    public async Task<IEnumerable<CabDataDto>> ProcessAsync(DataProcessorOptions options)
    {
        var dataInChunks = data.Chunk(options.BatchSize);

        if (options.ProcessInParallel)
        {
            var processTasks = dataInChunks.Select(ProcessBatchAsync).ToArray();
            await Task.WhenAll(processTasks);
        }
        else
        {
            foreach (var chunk in dataInChunks)
            {
                await ProcessBatchAsync(chunk);
            }
        }

        return _duplicateData;
    }

    private async Task ProcessBatchAsync(IEnumerable<CabDataDto> data)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        const string sql =
            """
            INSERT INTO dbo.CabData (
                 TpepPickupDateTime,
                 TpepDropoffDateTime,
                 PassengerCount,
                 TripDistance,
                 StoreAndFwdFlag,
                 PULocationId,
                 DOLocationId,
                 FareAmount,
                 TipAmount)
            VALUES (
                 @TpepPickupDateTime,
                 @TpepDropoffDateTime,
                 @PassengerCount,
                 @TripDistance,
                 @StoreAndFwdFlag,
                 @PULocationId,
                 @DOLocationId,
                 @FareAmount,
                 @TipAmount)
            """;
        
        foreach (var cabDataDto in data)
        {
            if (!_duplicates.TryAdd(
                (cabDataDto.TpepPickupDatetime, cabDataDto.TpepDropoffDatetime, cabDataDto.PassengerCount),
                true))
            {
                _duplicateData.Add(cabDataDto);
                continue;
            }
            
            await connection.ExecuteAsync(sql, cabDataDto);
        }
    }
}
