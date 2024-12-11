namespace DevelopsTodayTestTask;

public sealed class DataProcessorOptions
{
    public int BatchSize { get; init; }
    public bool ProcessInParallel { get; init; }

    public static DataProcessorOptions Default => new()
    {
        BatchSize = 1000,
        ProcessInParallel = false
    };

    public static DataProcessorOptions ParallelDefault => new()
    {
        BatchSize = 1000,
        ProcessInParallel = true
    };
}
